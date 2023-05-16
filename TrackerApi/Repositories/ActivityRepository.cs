using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using TrackerApi.Controllers;
using TrackerApi.Extensions;
using TrackerApi.Models;
using TrackerApi.Models.HelperModels;
using TrackerApi.Repositories.Interfaces;

namespace TrackerApi.Repositories
{
    public class ActivityRepository : BaseRepository<Activity>, IActivityRepository
    {
        public ActivityRepository(TrackerContext trackerContext, ILogger<ActivityRepository> logger, IMapper mapper) : base(trackerContext, logger, mapper)
        {
        }

        public Task<GeneralCustomerReport> CustomerGeneralReport(Guid customerUrl, bool preMonth = false, int timezoneOffset = 0)
        {
            var project = _trackerContext.Project.Include(x => x.UserProject).ThenInclude(x => x.User).FirstOrDefault(x => x.CustomerUrl == customerUrl);
            if (project == null)
            {
                return null;
            }
            var activityQuery = _trackerContext.Activity.Include(x => x.Project).Where(x => customerUrl == x.Project.CustomerUrl);
            var today = DateTime.UtcNow.AddMinutes(timezoneOffset);

            if (preMonth)
            {
                today = today.AddMonths(-1);
            }

            var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            activityQuery = activityQuery.Where(x => x.WorkedFromUtc.AddMinutes(timezoneOffset) > firstDayOfMonth && x.WorkedFromUtc < lastDayOfMonth);

            var activity = activityQuery.Include(x => x.User).ToArray();
            if (activity == null || activity.Length == 0)
            {
                return Task.FromResult(new GeneralCustomerReport { ProjectName = project.Name });
            }

            var userReport = activity.GroupBy(x => x.UserId).Select(
                item =>
                {
                    var activitys = item.ToArray();
                    var user = activitys.First().User;

                    return CreateUserCustomerReport(activitys, user, today);

                }).OrderBy(x=> x.UserId).ToArray();


            var activitiesReport = activity.Select(x =>
            {
                var time = x.WorkedFromUtc.AddMinutes(timezoneOffset);
                x.WorkedFromUtc = new DateTime(time.Year, time.Month, time.Day, 0, 0, 0);
                return x;
            })
             .GroupBy(c => new
             {
                 c.WorkedFromUtc,
                 c.UserId,
             }).Select(x =>
             {
                 var activitiesByUserAndDay = x.ToArray();
                 return CreateActivityCustomerReport(activitiesByUserAndDay);
             }).OrderBy(x => x.Date).ToArray();

            var report = CreateGeneralCustomerReport(userReport, activitiesReport, project.Name);
            return Task.FromResult(report);
        }


        private ActivityCustomerReport CreateActivityCustomerReport(Activity[] activity)
        {
            var activitiesByUserAndDayFirst = activity.First();
            var description = string.Join(", " + Environment.NewLine, activity.Select(x => x.Description));
            return new ActivityCustomerReport
            {
                Description = description,
                Duration = activity.Sum(x => x.Duration.Value),
                Date = activitiesByUserAndDayFirst.WorkedFromUtc,
                UserId = activitiesByUserAndDayFirst.UserId,
                UserName = activitiesByUserAndDayFirst.User.FirstName + " " + activitiesByUserAndDayFirst.User.LastName,
            };
        }

        private GeneralCustomerReport CreateGeneralCustomerReport(UserCustomerReport[] userReport, ActivityCustomerReport[] activityCustomerReports, string projectName)
        {
            var hours = userReport.Sum(x => x.TotalTime.Hours);
            var minutes = userReport.Sum(x => x.TotalTime.Minutes);
            var report = new GeneralCustomerReport
            {
                UserReports = userReport,
                ProjectName = projectName,
                TotalTime = new TimeCustomerReport
                {
                    Hours = hours + (int)(minutes / 60),
                    Minutes = (int)(minutes % 60)

                },
                Activities = activityCustomerReports
            };
            return report;
        }


        private UserCustomerReport CreateUserCustomerReport(Activity[] activitys, IdentityAuthUser user, DateTime today)
        {
            var totalMinutes = activitys.Sum(x => x.Duration);
            return new UserCustomerReport
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ReportForWeeks = CreateReportForWeeks(activitys, today),
                TotalTime = new TimeCustomerReport
                {
                    Hours = (int)(totalMinutes / 60),
                    Minutes = (int)(totalMinutes % 60)
                }
            };
        }

        private TimeCustomerReportForPeriod[] CreateReportForWeeks(Activity[] activitys, DateTime today)
        {
            var weeks = today.GetMonthWeekList();
            var weeksReport = new List<TimeCustomerReportForPeriod>();
            weeks.ForEach(week =>
            {
                var minutes = activitys.Where(x => week.From < x.WorkedFromUtc && week.To > x.WorkedFromUtc).Sum(x => x.Duration);
                weeksReport.Add(new TimeCustomerReportForPeriod
                {
                    From = week.From,
                    To = week.To,
                    Hours = (int)(minutes / 60),
                    Minutes = (int)(minutes % 60)
                });
            });
            return weeksReport.ToArray();
        }

    }



}