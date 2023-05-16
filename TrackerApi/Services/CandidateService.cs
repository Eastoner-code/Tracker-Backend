using AutoMapper;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrackerApi.ApiModels;
using TrackerApi.Extensions.Models;
using TrackerApi.Models;
using TrackerApi.Repositories.Interfaces;
using TrackerApi.Services.Interfaces;

namespace TrackerApi.Services
{
    public class CandidateService : BaseService<CandidateApiModel, Candidate>, ICandidateService
    {
        private ICandidateRepository _candidateRepository;

        public CandidateService(ICandidateRepository candidateRepository, IMapper mapper) : base(candidateRepository, mapper) 
        {
            _candidateRepository = candidateRepository;
        }

        public async Task<PagedResult<CandidateApiModel>> GetPageByVacancy(CandidateFilterApiModel filter)
        {
            return await _candidateRepository.GetPageAsync<CandidateApiModel>(x => x.VacancyId == filter.VacancyId, filter.Page, filter.PageSize, c => c.InterviewDate);
        }

        public async Task<PagedResult<CandidateApiModel>> GetCandidatePage(CandidatePageFilterApiModel pagination)
        {
            return await _candidateRepository.GetPageAsync<CandidateApiModel>(pagination.Page, pagination.PageSize);
        }
    }
}
