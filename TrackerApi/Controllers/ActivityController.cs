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
using TrackerApi.Validation;

namespace TrackerApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ActivityController : Controller
    {
        private readonly IActivityService _activityService;
        private readonly UserManager<IdentityAuthUser> _userManager;
        private readonly IUserService _userService;
        private readonly ActivityValidator _activityValidator;

        public ActivityController(IActivityService activityService, UserManager<IdentityAuthUser> userManager, IUserService userService, ActivityValidator activityValidator)
        {
            _activityService = activityService;
            _userManager = userManager;
            _userService = userService;
            _activityValidator = activityValidator;
        }

        /// <summary>
        ///     Create activity
        /// </summary>
        /// <param name="model">activity model</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ActivityApiModel), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> Create(ActivityApiModel model)
        {
            model.CreatedAtUtc = System.DateTime.UtcNow;
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (_activityValidator.Validate(model).IsValid == false) return NotFound();
            var res = await _activityService.CreateAsync(model);
            if (res == null) return NotFound();
            return Ok(res);
        }

        /// <summary>
        ///     Update activity
        /// </summary>
        /// <param name="model">activity model</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ActivityApiModel), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> Update(ActivityApiModel model)
        {
            var res = await _activityService.UpdateAsync(model);
            if (res == null) return NotFound();
            return Ok(res);
        }

        /// <summary>
        ///     Delete activity
        /// </summary>
        /// <param name="id">Id of activity</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(bool), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _activityService.DeleteAsync(id);
            return Ok(res);
        }


        /// <summary>
        ///     Return a single activity by id
        /// </summary>
        /// <param name="id">Id of activity</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ActivityApiModel), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> Get(int id)
        {
            var res = await _activityService.GetByIdAsync(id);
            if (res == null) return NotFound();
            return Ok(res);
        }

        /// <summary>
        ///     Return all activitys
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(List<ActivityApiModel>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            var res = await _activityService.GetAllAsync();
            if (res == null) return NotFound();
            return Ok(res);
        }


        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(List<ActivityApiModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetByFilter(ActivityFilterApiModel model)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userService.GetUserByUserId(userId);
            if (user != null)
            {
                int[] ids = { user.Id };
                var res = await _activityService.GetAllByFilterAsync(model, ids);
                if (res == null) return NotFound();
                return Ok(res);
            }
            return NotFound();
        }
        
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(PagedResult<ActivityApiModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPageByFilter(ActivityFilterPageApiModel model)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userService.GetUserByUserId(userId);
            if (user != null)
            {
                int[] ids = { user.UserId };
                var res = await _activityService.GetPageByFilterAsync(model, ids);
                if (res == null) return NotFound();
                return Ok(res);
            }
            return NotFound();
        }


        
    }
}