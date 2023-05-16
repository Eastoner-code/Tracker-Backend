using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TrackerApi.ApiModels;
using TrackerApi.Models;
using TrackerApi.Repositories.Interfaces;
using TrackerApi.Services.Interfaces;

namespace TrackerApi.Services
{
    public class UserRateService : BaseService<UserRateApiModel, UserRate>, IUserRateService
    {
        private readonly IUserRateRepository _rateRepository;
        public UserRateService(IUserRateRepository rateRepository, IMapper mapper) : base(rateRepository, mapper)
        {
            _rateRepository = rateRepository;
        }
        public async Task<IEnumerable<UserRateApiModel>> GetRatesByUserId(int userId)
        {
            var rates = (await _rateRepository.GetAllAsync(i => i.UserId == userId)).OrderByDescending(x=>x.Date);
            return rates == null ? null : _mapper.Map<IEnumerable<UserRateApiModel>>(rates);
        }

        public async Task<UserRateApiModel> GetRatesByDateUserId(int userId, DateTime dateTime)
        {
            var result = await _rateRepository.GetRatesByDateUserId(userId, dateTime);
            return result == null ? null : _mapper.Map<UserRateApiModel>(result);
        }

        public override async Task<CreateUpdate<UserRateApiModel>> CreateAsync(UserRateApiModel model)
        {
            model.Date = model.Date.Date.ToUniversalTime();
            return await base.CreateAsync(model);
        }

        public override async Task<CreateUpdate<UserRateApiModel>> UpdateAsync(UserRateApiModel model)
        {
            model.Date = model.Date.Date.ToUniversalTime();
            return await base.UpdateAsync(model);
        }
    }
}