using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrackerApi.ApiModels;

namespace TrackerApi.Services.Interfaces
{
    public interface IHolidayService : IBaseService<HolidayApiModel>
    {
        Task<IEnumerable<HolidayApiModel>> GetHolidaysByYears(DateTime from, DateTime to);
        Task<bool> UpdateHolidays(IEnumerable<HolidayApiModel> holidays, DateTime from, DateTime to);
    }
}