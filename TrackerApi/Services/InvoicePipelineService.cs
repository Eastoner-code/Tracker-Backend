using AutoMapper;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using MimeKit;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerApi.ApiModels;
using TrackerApi.Extensions.Models;
using TrackerApi.Models;
using TrackerApi.Repositories.Interfaces;
using TrackerApi.Services.Interfaces;

namespace TrackerApi.Services
{
    public class InvoicePipelineService : BaseService<InvoicePipelineApiModel, InvoicePipeline>, IInvoicePipelineService
    {
        private readonly IActivityService _activityService;
        private readonly IInvoiceUserProjectRepository _invoiceUserProjectRepository;
        private readonly IPaymentDetailsRepository _paymentDetailsRepository;
        public InvoicePipelineService(IInvoicePipelineRepository repository, IMapper mapper, IActivityService activityService, IInvoiceUserProjectRepository invoiceUserProjectRepository, IPaymentDetailsRepository paymentDetailsRepository) : base(repository, mapper)
        {
            _activityService = activityService;
            _invoiceUserProjectRepository = invoiceUserProjectRepository;
            _paymentDetailsRepository = paymentDetailsRepository;
        }
        [Obsolete]
        public async Task CreatePdfInvoiceAsync(int invoiceId, int paymentId)
        {
            var invoice = await _repository.GetByAsync(inv => inv.Id == invoiceId);
            if (invoice == null) throw new RecordNotFoundException("No invoices found");
            var invoiceUserProject = await _invoiceUserProjectRepository.GetAllAsync(inv => inv.InvoiceId == invoiceId);
            var activityFilter = new ActivityFilterApiModel()
            {
                StartDate = invoice.StartDate,
                EndDate = invoice.EndDate,
                ProjectIds = invoiceUserProject.Select(x => x.ProjectId).ToArray()
            };
            var devIds = invoiceUserProject.Select(x => x.UserId).ToArray();
            var activities = await _activityService.GetAllByInvoiceAsync(invoiceUserProject);
            var payment = await _paymentDetailsRepository.GetByAsync(p => p.Id == paymentId);

            Dictionary<string, string> htmlItems = GethtmlItems(invoice, payment);
            var strHtml = GetHTMLString(htmlItems, activities, invoiceUserProject, invoice);

            var revisionInfo = await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                ExecutablePath = revisionInfo.ExecutablePath
            });
            using (Page page = browser.NewPageAsync().Result)
            {
                await page.SetContentAsync(strHtml);
                await page.AddStyleTagAsync(new AddTagOptions { Path = Path.Combine(Environment.CurrentDirectory, @"InvoiceTemplates\style.css") });
                var pdfStream = await page.PdfStreamAsync(new PdfOptions { Format = PaperFormat.A4 });
                var result = SendEmailWithPdf(pdfStream, invoice);
                if (result.IsCompletedSuccessfully == true)
                {
                    invoice.IsSent = true;
                    await _repository.UpdateAsync(invoice);
                }
            }

        }

        private Dictionary<string, string> GethtmlItems(InvoicePipeline invoice, PaymentDetails payment)
        {
            var htmlItems = new Dictionary<string, string>();
            htmlItems.Add("ContactPersonNameUA",payment.ContactPersonNameUA);
            htmlItems.Add("ContactPersonNameENG",payment.ContactPersonNameENG);
            htmlItems.Add("ContactPersonAdressUA",payment.AdressUA);
            htmlItems.Add("ContactPersonAdressENG",payment.AdressENG);
            htmlItems.Add("InvoiceCreated", invoice.CreatedAtUtc.ToShortDateString());
            htmlItems.Add("CustomerName", invoice.CustomerName);
            htmlItems.Add("CustomerAdressUA", invoice.CustomerAdressUA);
            htmlItems.Add("CustomerAdressENG", invoice.CustomerAdressENG);
            htmlItems.Add("InvoiceNumber", invoice.InvoiceNumber);
            htmlItems.Add("PaymentDate", GetPaymentDate());
            htmlItems.Add("PaymentDetails",payment.Details);
            return htmlItems;
        }

        [Obsolete]
        private async Task SendEmailWithPdf(Stream pdfStream, InvoicePipeline invoice)
        {
            var message = new MimeMessage();
            //message.From.Add(new MailboxAddress("Name","Email")); //should be mail service email
            message.To.Add(new MailboxAddress(invoice.CustomerEmail));
            message.Subject = $"Invoice_{invoice.InvoiceNumber}";
            var emailBody = new BodyBuilder();
            emailBody.Attachments.Add($"Invoice_{invoice.InvoiceNumber}.pdf", pdfStream);
            message.Body = emailBody.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.Connect("eastoner.net", 465, true);
                await client.SendAsync(message);
                client.Disconnect(true);
            }
        }

        private string GetHTMLString(Dictionary<string,string> htmlItems, List<ActivityApiModel> activities, List<InvoiceUserProject> invoiceUserProjects, InvoicePipeline invoice)
        {
            var pathHtml = Path.Combine(Environment.CurrentDirectory, @"InvoiceTemplates\InvoiceTemplate.html");
            string htmlString = File.ReadAllText(pathHtml);
            
            var pathItem = Path.Combine(Environment.CurrentDirectory, @"InvoiceTemplates\item.html");
            string itemString = File.ReadAllText(pathItem);
            
            var stringBuilder = new StringBuilder();
            var itemBuilder = new StringBuilder();
            stringBuilder.Append(htmlString);
            
            foreach(var item in htmlItems)
            {
                stringBuilder.Replace(item.Key, item.Value);
            }
            
            var rates = invoiceUserProjects.GroupBy(inv => inv.Rate);
            var subtotal = new int();
            var counter = 1;
            foreach (var rate in rates)
            {
                var activitiesByRate = activities.FindAll(act => rate.All(r => r.ProjectId == act.ProjectId && r.UserId == act.UserId));
                var units = activitiesByRate.Sum(act => Convert.ToDouble(act.Duration)) / 60;//convert to hours
                var pricePerUnit = Convert.ToDouble(rate.Key);
                var amount = units * pricePerUnit;
                subtotal += Convert.ToInt32(amount);
                itemBuilder.Append(string.Format(itemString,counter,pricePerUnit,units,amount));
                counter++;
            }
            var total = subtotal;
            stringBuilder.Replace("Items",itemBuilder.ToString());
            stringBuilder.Replace("{subtotal}",subtotal.ToString());
            stringBuilder.Replace("{total}", total.ToString());
            invoice.SubTotal = subtotal;
            invoice.Total = total;
            _repository.UpdateAsync(invoice);
            return stringBuilder.ToString();
        }
        public async Task<InvoicePipelineApiModel> GenerateInvoicePipelineAsync(InvoicePipelineApiModel model)
        {
            model.InvoiceNumber = CreateInvoiceNumber();
            var createdModel = await CreateAsync(model);
            var list = new List<InvoiceUserProject>();
            model.Projects
                .ForEach(p => p.Developers
                .ForEach(dev => list
                .Add(new InvoiceUserProject { Id = 0, InvoiceId = createdModel.Model.Id, ProjectId = p.ProjectId, UserId = dev.DeveloperId, Rate = dev.Rate })));
            await _invoiceUserProjectRepository.CreateRangeAsync(list);
            return model;
        }
        public async Task<PagedResult<InvoicePipelineApiModel>> GetAllPaginated(int page, int pageSize)
        {
            var invoices = await _repository.GetPageAsync<InvoicePipelineApiModel>(page, pageSize);
            if (invoices == null) throw new RecordNotFoundException("No invoices found");
            return invoices;
        }
        private string GetPaymentDate()
        {
            var paymentDate = DateTime.UtcNow;
            var date = paymentDate;
            var addedDays = 0;
            while (addedDays < 10)
            {
                if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                {
                    paymentDate = date.AddDays(1);
                    addedDays++;
                }
                date = date.AddDays(1);
            }
            return paymentDate.Date.ToShortDateString();
        }
        private string CreateInvoiceNumber()
        {
            var today = DateTime.UtcNow;
            var countInvoicesToday = _repository.GetAllAsync(inv => inv.CreatedAtUtc.Date == today.Date).Result.Count;
            return today.Year.ToString() + today.Month.ToString() + today.Day.ToString() + "_" + Convert.ToString(countInvoicesToday + 1);
        }
    }
}
