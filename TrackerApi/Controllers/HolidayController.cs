using System;
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
    public class HolidayController : Controller
    {
        private readonly IHolidayService _holidayService;

        public HolidayController(IHolidayService holidayService)
        {
            _holidayService = holidayService;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<HolidayApiModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetHolidaysByYears(DateTime from, DateTime to)
        {
            var res = await _holidayService.GetHolidaysByYears(from, to);
            if (res == null) return NotFound();
            return Ok(res);
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateHolidays(DateTime from, DateTime to, [FromBody] IEnumerable<HolidayApiModel> holidays)
        {
            var res = await _holidayService.UpdateHolidays(holidays, from, to);
            return Ok(res);
        }
    }
}
