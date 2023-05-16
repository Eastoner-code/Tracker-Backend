using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrackerApi.ApiModels;
using TrackerApi.Extensions.Models;
using TrackerApi.Models;

namespace TrackerApi.Services.Interfaces
{
    public interface IInvoicePipelineService : IBaseService<InvoicePipelineApiModel>
    {
        Task<InvoicePipelineApiModel> GenerateInvoicePipelineAsync(InvoicePipelineApiModel model);
        Task<PagedResult<InvoicePipelineApiModel>> GetAllPaginated(int page, int pageSize);
        Task CreatePdfInvoiceAsync(int invoiceId, int paymentId);
    }
}
