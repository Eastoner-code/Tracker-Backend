using AutoMapper;
using Microsoft.Extensions.Logging;
using TrackerApi.Models;
using TrackerApi.Repositories.Interfaces;

namespace TrackerApi.Repositories
{
    public class SkillRepository : BaseRepository<Skill>, ISkillRepository
    {
        public SkillRepository(TrackerContext trackerContext, ILogger<SkillRepository> logger, IMapper mapper) : base(trackerContext, logger, mapper)
        {
        }
    }
}