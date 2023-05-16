using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrackerApi.ApiModels;
using TrackerApi.Extensions.Models;
using TrackerApi.Models;
using TrackerApi.Repositories.Interfaces;
using TrackerApi.Services.Interfaces;

namespace TrackerApi.Services
{
    public class VacancyService : BaseService<VacancyApiModel, Vacancy>, IVacancyService
    {
        IVacancyRepository _vacancyRepository;
        
        public VacancyService(IVacancyRepository vacancyRepository, IMapper mapper) : base(vacancyRepository, mapper)
        {
            _vacancyRepository = vacancyRepository;
        }

        public async Task<IEnumerable<VacancyApiModel>> AllVacancies()
        {
            var notifications = await _vacancyRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<VacancyApiModel>>(notifications);
        }

        public async Task<PagedResult<VacancyApiModel>> VacanciesPage(VacancyPageFilterApiModel pagination)
        {
            return await _vacancyRepository.GetPageAsync<VacancyApiModel>(pagination.Page, pagination.PageSize);

        }
    }
}
