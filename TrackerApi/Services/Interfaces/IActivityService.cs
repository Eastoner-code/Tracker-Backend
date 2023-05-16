using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrackerApi.ApiModels;
using TrackerApi.Extensions.Models;
using TrackerApi.Models;
using TrackerApi.Models.Enums;
using TrackerApi.Repositories;

namespace TrackerApi.Services.Interfaces
{
    public interface IActivityService : IBaseService<ActivityApiModel>
    {
        Task<List<ActivityApiModel>> GetAllByFilterAsync(ActivityFilterApiModel model, int[] userId);
        Task<List<ActivityApiModel>> GetAllByFilterTrackedAsync(DateTime startDate, DateTime endDate, int userId);

        Task<PagedResult<ActivityApiModel>> GetPageByFilterAsync(ActivityFilterPageApiModel model, int[] userId);

        Task<GeneralCustomerReportApiModal> CustomerGeneralReport(Guid customerUrl, bool preMonth = false, int timezoneOffset = 0);
        Task<Task> CheckUserTrackings(int userId);
        Task<List<ActivityApiModel>> GetAllByInvoiceAsync(List<InvoiceUserProject> invoiceUserProject);
    }
}