using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackerApi.Extensions;

namespace TrackerApi.ApiModels
{
    public class ReportApiModel
    {
        public List<ActivityApiModel> Activities;
        public List<TimeSpan> TotalTime;
        public ReportApiModel(List<ActivityApiModel> activities,string userTimeZone)
        {
            Activities = activities;
            TotalTime = CalculateTotalTime(activities,userTimeZone);
        }
        private List<TimeSpan> CalculateTotalTime(List<ActivityApiModel> activities, string userTimeZone)
        {
            var totalTime = new List<TimeSpan>();
            var timeZone = DateTimeZoneProviders.Tzdb[userTimeZone];
            var startDate = activities.First().WorkedFromUtc.ConvertToLocalDateTimeWithoutTime(timeZone);
            var endDate = activities.Last().WorkedToUtc.ConvertToLocalDateTimeWithoutTime(timeZone);
            for (LocalDateTime day = startDate; day.Date <= endDate.Date; day = day.PlusHours(24))
            {
                var activitiesByDay = activities.FindAll(x => (x.WorkedFromUtc.ConvertToLocalDateTime(timeZone) >= day && x.WorkedToUtc.ConvertToLocalDateTime(timeZone) <= day.PlusHours(24)));
                totalTime.Add(TimeSpan.FromMinutes(activitiesByDay.Sum(act => Convert.ToDouble(act.Duration))));
            }
            return totalTime;
        }
    }
}
