using System.Collections.Generic;
using System.Threading.Tasks;
using TrackerApi.ApiModels;
using TrackerApi.Extensions.Models;

namespace TrackerApi.Services.Interfaces
{
    public interface IVacancyService : IBaseService<VacancyApiModel>
    {
        Task<PagedResult<VacancyApiModel>> VacanciesPage(VacancyPageFilterApiModel pagination);
        Task<IEnumerable<VacancyApiModel>> AllVacancies();
    }
}
