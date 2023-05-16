using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrackerApi.ApiModels;

namespace TrackerApi.Services.Interfaces
{
    public interface IUserRateService : IBaseService<UserRateApiModel>
    {
        Task<IEnumerable<UserRateApiModel>> GetRatesByUserId(int userId);
        Task<UserRateApiModel> GetRatesByDateUserId(int userId, DateTime dateTime);
    }
}