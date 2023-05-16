using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using TrackerApi.ApiModels;
using TrackerApi.Services.Interfaces;

namespace TrackerApi.Controllers
{
    [Authorize(Roles = "Admin, SuperAdmin, Supervisor")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class InvoicePipelineController : Controller
    {
        private readonly IInvoicePipelineService _invoicePipelineService;
        public InvoicePipelineController(IInvoicePipelineService invoicePipelineService)
        {
            _invoicePipelineService = invoicePipelineService;
        }
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(InvoicePipelineApiModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Create(InvoicePipelineApiModel model)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            model.UserId = Convert.ToInt32(userId);
            var res = await _invoicePipelineService.GenerateInvoicePipelineAsync(model);
            if (res == null) return NotFound();
            return Ok(res);
        }
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(List<InvoicePipelineApiModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll(int page, int pageSize)
        {
            var res = await _invoicePipelineService.GetAllPaginated(page,pageSize);
            if (res == null) return NotFound();
            return Ok(res);
        }
        [HttpGet]
        public async Task<IActionResult> CreatePDF(int invoiceId, int paymentId)
        {
            await _invoicePipelineService.CreatePdfInvoiceAsync(invoiceId,paymentId);
            return Ok();

        }
    }
}
