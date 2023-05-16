using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackerApi.ApiModels;
using TrackerApi.Services.Interfaces;

namespace TrackerApi.Controllers
{
    [Authorize(Roles = "Admin, SuperAdmin, User, AbsenceApprover")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SalaryCalculationController : Controller
    {
        private readonly ISalaryCalculationService _salaryCalculationService;
        public SalaryCalculationController(ISalaryCalculationService salaryCalculationService)
        {
            _salaryCalculationService = salaryCalculationService;
        }

        [HttpGet("{userId}/{month}/{year}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MonthSalaryApiModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetSalaryByUserIdMonth(int userId, int month, int year)
        {
            var result = await _salaryCalculationService.GetSalaryByUserIdMonth(userId, month, year);
            return Ok(result);
        }

        [HttpGet("{month}/{year}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<SalariesApiModel>), (int)HttpStatusCode.OK)]
        public IActionResult GetSalariesByMonth(int month, int year)
        {
            return Ok();
        }
    }
}
