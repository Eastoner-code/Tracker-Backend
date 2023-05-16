using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using TrackerApi.ApiModels;
using TrackerApi.Extensions.Models;
using TrackerApi.Models.Enums;
using TrackerApi.Services.Interfaces;

namespace TrackerApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RecruitmentController : Controller
    {
        private IVacancyService _vacancyService;
        private ICandidateService _candidateService;

        public RecruitmentController(IVacancyService vacancyService, ICandidateService candidateService)
        {
            _vacancyService = vacancyService;
            _candidateService = candidateService;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<VacancyApiModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AllVacancies()
        {
            var allVacancies = await _vacancyService.GetAllAsync();
            if (allVacancies == null) return NotFound();
            return Ok(allVacancies);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(PagedResult<VacancyApiModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> VacanciesPage(VacancyPageFilterApiModel pagination)
        {
            var res = await _vacancyService.VacanciesPage(pagination);
            if (res == null) return NotFound();
            return Ok(res);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(PagedResult<CandidateApiModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> VacancyCandidates(CandidateFilterApiModel filter)
        {
            var res = await _candidateService.GetPageByVacancy(filter);
            if (res == null) return NotFound();
            return Ok(res);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(PagedResult<CandidateApiModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CadnditatesPage(CandidatePageFilterApiModel pagination)
        {
            var res = await _candidateService.GetCandidatePage(pagination);
            if (res == null) return NotFound();
            return Ok(res);
        }

        [HttpPut("{id}/{status}")]
        [Authorize(Roles = "Admin, SuperAdmin, Recruiter")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(CreateUpdate<CandidateApiModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateStatus(int id, CandidateStatus status)
        {
            throw new System.NotImplementedException();
        }

    }
}
