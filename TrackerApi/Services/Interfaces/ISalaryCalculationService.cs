using System.Threading.Tasks;
using TrackerApi.ApiModels;

namespace TrackerApi.Services.Interfaces
{
    public interface ISalaryCalculationService
    {
        Task<MonthSalaryApiModel> GetSalaryByUserIdMonth(int userId, int month, int year);
    }
}
