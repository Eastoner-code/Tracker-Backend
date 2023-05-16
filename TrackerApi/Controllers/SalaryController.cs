using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackerApi.ApiModels;
using TrackerApi.Services.Interfaces;

namespace TrackerApi.Controllers
{
    [Authorize(Roles = "Admin, SuperAdmin")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SalaryController : Controller
    {
        private readonly IUserRateService _rateService;

        public SalaryController(IUserRateService rateService)
        {
            _rateService = rateService;
        }

        [HttpGet("{userId}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<UserRateApiModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetRatesByUserId(int userId)
        {
            return Ok(await _rateService.GetRatesByUserId(userId));
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(UserRateApiModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddRate(UserRateApiModel model)
        {
            var res = await _rateService.CreateAsync(model);
            if (res == null) return NotFound();
            return Ok(res);
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(UserRateApiModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateRate(UserRateApiModel model)
        {
            var res = await _rateService.UpdateAsync(model);
            if (res == null) return NotFound();
            return Ok(res);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteRate(int id)
        {
            var res = await _rateService.DeleteAsync(id);
            return Ok(res);
        }
    }
}
