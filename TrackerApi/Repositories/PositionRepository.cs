using AutoMapper;
using Microsoft.Extensions.Logging;
using TrackerApi.Models;
using TrackerApi.Repositories.Interfaces;

namespace TrackerApi.Repositories
{
    public class PositionRepository : BaseRepository<Position>, IPositionRepository
    {
        public PositionRepository(TrackerContext trackerContext, ILogger<PositionRepository> logger, IMapper mapper) : base(trackerContext, logger, mapper)
        {
        }
    }
}