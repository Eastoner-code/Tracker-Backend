using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
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
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly IActivityService _activityService;
        private readonly int alertRetrieveAmount = 3;

        public NotificationController(INotificationService notificationService, IActivityService activityService)
        {
            _notificationService = notificationService;
            _activityService = activityService;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(NotificationAlertResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UserAlertNotifications()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await _notificationService.GetUserAlertNotifications(Convert.ToInt32(userId), alertRetrieveAmount);
            if (res == null) return NotFound();
            return Ok(res);
        }


        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(PagedResult<NotificationApiModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UserNotifications(NotificationFilterApiModel filter)
        {
            filter.UserId = Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            await _activityService.CheckUserTrackings(filter.UserId);
            var res = await _notificationService.GetNotificationPage(filter);
            if (res == null) return NotFound();
            return Ok(res);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(NotificationApiModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Create(NotificationApiModel notification)
        {
            var res = await _notificationService.CreateAsync(notification);
            if (res == null) return NotFound();
            return Ok(res);
        }


        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _notificationService.DeleteAsync(id);
            return Ok(res);
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(NotificationApiModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> MarkAsRead(int notificationId)
        {
            var notification = await _notificationService.GetByIdAsync(notificationId);

            notification.IsRead = true;

            var res = await _notificationService.UpdateAsync(notification);

            if (res == null) return NotFound();

            return Ok(res);
        }
    }
}
