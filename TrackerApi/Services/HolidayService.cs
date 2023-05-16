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
    public class HolidayService : BaseService<HolidayApiModel, Holiday>, IHolidayService
    {
        private readonly IHolidayRepository _holidayRepository;
        public HolidayService(IHolidayRepository holidayRepository, IMapper mapper) : base(holidayRepository, mapper)
        {
            _holidayRepository = holidayRepository;
        }

        public async Task<IEnumerable<HolidayApiModel>> GetHolidaysByYears(DateTime from, DateTime to)
        {
            var fromUtc = from.ToUniversalTime();
            var toUtc = to.ToUniversalTime();
            var holidays = await _holidayRepository.GetAllAsync(x => fromUtc <= x.Date && x.Date <= toUtc);
            return holidays == null ? null : _mapper.Map<IEnumerable<HolidayApiModel>>(holidays);
        }

        public async Task<bool> UpdateHolidays(IEnumerable<HolidayApiModel> holidays, DateTime from, DateTime to)
        {
            var fromUtc = from.ToUniversalTime();
            var toUtc = to.ToUniversalTime();
            var holidaysToDelete = await _holidayRepository.GetAllAsync(x => fromUtc <= x.Date && x.Date <= toUtc);

            await _holidayRepository.DeleteRangeAsync(holidaysToDelete);

            var modelsToAdd = _mapper.Map<IEnumerable<Holiday>>(holidays).ToArray();
            await _holidayRepository.CreateRangeAsync(modelsToAdd);

            return true;
        }
    }
}