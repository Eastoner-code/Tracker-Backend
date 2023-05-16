using System;
using System.Threading.Tasks;
using TrackerApi.Models;

namespace TrackerApi.Repositories.Interfaces
{
    public interface IUserYearRangeRepository : IBaseRepository<UserYearRange>
    {
        Task<UserYearRange> GetRangeByStartDate(DateTime date, int userId);
        Task<UserYearRange> GetCurrentRange(int userId);
        Task<UserYearRange> GetRangeWithAbsences(int rangeId);
    }
}