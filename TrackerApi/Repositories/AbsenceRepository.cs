using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TrackerApi.Models;
using TrackerApi.Repositories.Interfaces;

namespace TrackerApi.Repositories
{
    public class AbsenceRepository : BaseRepository<Absence>, IAbsenceRepository
    {
        public AbsenceRepository(TrackerContext trackerContext, ILogger<BaseRepository<Absence>> logger, IMapper mapper) : base(trackerContext, logger, mapper)
        {
        }

        public virtual Task<List<Absence>> GetAllIncludeUserAsync(Func<Absence, bool> filter)
        {
            var result = _dbSet.AsNoTracking().Include(x => x.User).Where(filter).ToList();
            return Task.FromResult(result);
        }
    }
}