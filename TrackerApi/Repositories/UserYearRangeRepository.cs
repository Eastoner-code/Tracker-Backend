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
    public class UserYearRangeRepository : BaseRepository<UserYearRange>, IUserYearRangeRepository
    {
        public UserYearRangeRepository(TrackerContext trackerContext, ILogger<BaseRepository<UserYearRange>> logger, IMapper mapper) : base(trackerContext, logger, mapper)
        {
        }

        public async Task<UserYearRange> GetRangeByStartDate(DateTime date, int userId)
        {
            return await _dbSet.AsQueryable().Where(x => x.UserId == userId && x.From <= date && date <= x.To).FirstOrDefaultAsync();
        }

        public async Task<UserYearRange> GetCurrentRange(int userId)
        {
            var utcNow = DateTime.UtcNow;
            return await _dbSet.AsQueryable().Where(x => x.UserId == userId && x.From <= utcNow && utcNow <= x.To).FirstOrDefaultAsync();
        }

        public async Task<UserYearRange> GetRangeWithAbsences(int rangeId)
        {
            return await _dbSet.Include(x => x.Absences).Where(x => x.Id == rangeId).FirstOrDefaultAsync();
        }
    }
}