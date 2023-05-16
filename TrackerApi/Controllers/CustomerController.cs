using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TrackerApi.ApiModels;
using TrackerApi.Repositories;
using TrackerApi.Repositories.Interfaces;
using TrackerApi.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TrackerApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CustomerController : Controller
    {
        private IActivityService _activityService;
        public CustomerController(IActivityService activityService)
        {
            _activityService = activityService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerUrl"></param>
        /// <param name="preMonth"></param>
        /// <param name="timezoneOffset"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(GeneralCustomerReportApiModal), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CustomerReport([FromQuery]Guid customerUrl, [FromQuery]bool preMonth = false, int timezoneOffset = 0)
        {
            var res =  await _activityService.CustomerGeneralReport(customerUrl, preMonth, timezoneOffset);
            if (res == null) return NotFound();
            return Ok(res);
        }
    }
}
