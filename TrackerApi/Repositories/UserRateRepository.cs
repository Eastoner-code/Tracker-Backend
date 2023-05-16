using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TrackerApi.Models;
using TrackerApi.Repositories.Interfaces;

namespace TrackerApi.Repositories
{
    public class UserRateRepository : BaseRepository<UserRate>, IUserRateRepository
    {
        public UserRateRepository(TrackerContext trackerContext, ILogger<BaseRepository<UserRate>> logger, IMapper mapper) : base(trackerContext, logger, mapper)
        {
        }

        public async Task<UserRate> GetRatesByDateUserId(int userId, DateTime dateTime)
        {
            var rates = await _trackerContext.UserRates.AsNoTracking().Where(i => i.UserId == userId)
                .ToListAsync();

            return rates.Where(d => (d.Date - dateTime).Ticks < 0)
                .OrderByDescending(d => (d.Date - dateTime).Ticks).FirstOrDefault();
        }
    }
}