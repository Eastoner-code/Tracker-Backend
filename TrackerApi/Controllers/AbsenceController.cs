using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackerApi.ApiModels;
using TrackerApi.Models.Enums;
using TrackerApi.Services.Interfaces;

namespace TrackerApi.Controllers
{
    [Authorize(Roles = "Admin, SuperAdmin, AbsenceApprover, User, Supervisor")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AbsenceController : Controller
    {
        private readonly IAbsenceService _absenceService;
        private readonly IUserService _userService;
        private readonly IUserYearRangeService _userYearRangeService;
        private readonly INotificationService _notificationService;

        public AbsenceController(IAbsenceService absenceService, IUserService userService, IUserYearRangeService userYearRangeService, INotificationService notificationService)
        {
            _absenceService = absenceService;
            _userService = userService;
            _userYearRangeService = userYearRangeService;
            _notificationService = notificationService;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<UserYearRangeApiModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetMyRanges()
        {
            await _userYearRangeService.CheckRangesAndAbsences();
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userService.GetUserByUserId(userId);
            if (user == null) return NotFound();
            var result = await _userYearRangeService.GetRangesByUserId(user.Id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("{rangeId}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(UserYearRangeFullApiModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDaysSummary(int rangeId)
        {
            var result = await _userYearRangeService.GetDaysSummaryByRangeId(rangeId);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin, AbsenceApprover")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<AbsenceApiModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllAbsencesByRange(FromToApiModel model)
        {
            var result = await _absenceService.GetAllAbsencesByRange(model.From, model.To);
            if (result == null) return NotFound();
            return Ok(result);
        }


        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<AbsenceApiModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetMyAbsences(int yearRangeId)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userService.GetUserByUserId(userId);
            if (user == null) return NotFound();
            var result = await _absenceService.GetMyAbsences(yearRangeId, user.Id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(CreateUpdate<AbsenceApiModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> RequestAbsence(AbsenceApiModel model)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userService.GetUserByUserId(userId);
            if (user == null) return NotFound();
            model.UserId = user.Id;
            var result = await _absenceService.RequestAsync(model);
            await _notificationService.NotifyAllAbsenceApprovers(result.Model);

            return Ok(result);
        }


        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(AbsenceApiModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(int id)
        {
            var absence = await _absenceService.GetByIdAsync(id);
            if (absence == null || absence.Status != AbsenceStatus.Pending)
                return BadRequest("Absence not found or can't be removed.");
            var res = await _absenceService.DeleteAndReturnDaysAsync(id);
            return Ok(res);

        }

        [HttpGet]
        [Authorize(Roles = "Admin, SuperAdmin, AbsenceApprover")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<AbsenceApiModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAbsencesToApprove()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userService.GetUserByUserId(userId);
            if (user == null) return NotFound();
            var result = await _absenceService.GetAbsencesToApprove(user.Id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPut("{id}/{status}")]
        [Authorize(Roles = "Admin, SuperAdmin, AbsenceApprover")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(CreateUpdate<AbsenceApiModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateRequestStatus(int id, AbsenceStatus status)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userService.GetUserByUserId(userId);
            if (user == null) return NotFound();
            var result = await _absenceService.UpdateRequestStatus(user.Id, id, status);
            if (result == null) return BadRequest();

            await _notificationService.AbsenceUpdateNotification(result.Model);
            return Ok(result);
        }
    }
}
