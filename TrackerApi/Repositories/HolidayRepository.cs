using AutoMapper;
using Microsoft.Extensions.Logging;
using TrackerApi.Models;
using TrackerApi.Repositories.Interfaces;

namespace TrackerApi.Repositories
{
    public class HolidayRepository : BaseRepository<Holiday>, IHolidayRepository
    {
        public HolidayRepository(TrackerContext trackerContext, ILogger<BaseRepository<Holiday>> logger, IMapper mapper) : base(trackerContext, logger, mapper)
        {
        }
    }
}