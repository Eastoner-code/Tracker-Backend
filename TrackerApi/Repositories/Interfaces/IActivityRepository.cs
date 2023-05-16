using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackerApi.Models;
using TrackerApi.Models.HelperModels;

namespace TrackerApi.Repositories.Interfaces
{
    public interface IActivityRepository: IBaseRepository<Activity>
    {
        Task<GeneralCustomerReport> CustomerGeneralReport(Guid customerUrl, bool preMonth = false, int timezoneOffset = 0);
    }
}