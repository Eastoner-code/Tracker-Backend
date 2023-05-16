using System;
using System.Threading.Tasks;
using TrackerApi.Models;

namespace TrackerApi.Repositories.Interfaces
{
    public interface IUserRateRepository : IBaseRepository<UserRate>
    {
        Task<UserRate> GetRatesByDateUserId(int userId, DateTime dateTime);
    }
}