using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using NodaTime;
using System;
using System.Security.Claims;
using TrackerApi.ApiModels;
using TrackerApi.Extensions;
using TrackerApi.Models;
using TrackerApi.Repositories.Interfaces;
using TrackerApi.Services.Interfaces;

namespace TrackerApi.Validation
{
    public class ActivityValidator : AbstractValidator<ActivityApiModel>
    {
        private readonly IActivityService _activityService;
        private readonly IUserService _userService;
        public ActivityValidator(IActivityService activityService, IUserService userService)
        {
            _activityService = activityService;
            _userService = userService;
            RuleFor(activity => activity.WorkedFromUtc).LessThan(activity => activity.WorkedToUtc);
            RuleFor(activity => activity.UserId).Must(ActivitiesCountLessThan24);
        }
        public bool ActivitiesCountLessThan24(int userId)
        { 
            var timeZone = DateTimeZoneProviders.Tzdb[_userService.GetByIdAsync(userId).Result.TimeZone];
            DateTime startDate = DateTime.UtcNow.ConvertToLocalDateTimeWithoutTime(timeZone).ToDateTimeUnspecified().Date;
            DateTime endDate = DateTime.UtcNow.ConvertToLocalDateTimeWithoutTime(timeZone).ToDateTimeUnspecified().AddDays(1).Date;
            var countActivities = _activityService.GetAllByFilterTrackedAsync(startDate, endDate, userId).Result.Count;
            if (countActivities < 24) return true;
            else return false;
        }
    }
}
