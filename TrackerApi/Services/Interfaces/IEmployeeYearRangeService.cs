using System.Collections.Generic;
using System.Threading.Tasks;
using TrackerApi.ApiModels;
using TrackerApi.Models;

namespace TrackerApi.Services.Interfaces
{
    public interface IUserYearRangeService: IBaseService<UserYearRangeApiModel>
    {
        Task CheckRangesAndAbsences();
        Task<IEnumerable<UserYearRangeApiModel>> GetRangesByUserId(int userId);
        Task<bool> SubtractAbsence(Absence absenceApiModel);
        Task<bool> ReturnDays(Absence absence);
        Task<UserYearRangeFullApiModel> GetDaysSummaryByRangeId(int rangeId);
    }
}