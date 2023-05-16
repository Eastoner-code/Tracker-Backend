using AutoMapper;
using Microsoft.Extensions.Logging;
using TrackerApi.Models;
using TrackerApi.Repositories.Interfaces;

namespace TrackerApi.Repositories
{
    public class VacancyRepository : BaseRepository<Vacancy>, IVacancyRepository
    {
        public VacancyRepository(TrackerContext trackerContext, ILogger<BaseRepository<Vacancy>> logger, IMapper mapper) : base(trackerContext, logger, mapper) {}
    }
}
