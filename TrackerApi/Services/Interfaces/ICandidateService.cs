using System.Collections.Generic;
using System.Threading.Tasks;
using TrackerApi.ApiModels;
using TrackerApi.Extensions.Models;

namespace TrackerApi.Services.Interfaces
{
    public interface ICandidateService : IBaseService<CandidateApiModel> 
    {
        Task<PagedResult<CandidateApiModel>> GetPageByVacancy(CandidateFilterApiModel filter);

        Task<PagedResult<CandidateApiModel>> GetCandidatePage(CandidatePageFilterApiModel pagination);
    }
}
