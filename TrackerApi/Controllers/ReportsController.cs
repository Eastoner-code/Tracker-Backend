using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TrackerApi.ApiModels;
using TrackerApi.Models;
using TrackerApi.Services.Interfaces;
using TrackerApi.Extensions.Models;

namespace TrackerApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ReportsController : Controller
    {
        private readonly IActivityService _activityService;
        private readonly UserManager<IdentityAuthUser> _userManager;

        public ReportsController(IActivityService activityService, UserManager<IdentityAuthUser> userManager)
        {
            _activityService = activityService;
            _userManager = userManager;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<ActivityApiModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetByFilter(ReportFilterApiModel model)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var res = await _activityService.GetAllByFilterAsync(model, model.UserIds);
                if (res == null) return NotFound();
                return Ok(res);
            }
            return NotFound();
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ReportApiModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetByFilterReport(ReportFilterApiModel model)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var res = await _activityService.GetAllByFilterAsync(model, model.UserIds);
                var report = new ReportApiModel(res,user.TimeZone);
                if (report == null) return NotFound();
                return Ok(report);
            }
            return NotFound();
        }



    }
}