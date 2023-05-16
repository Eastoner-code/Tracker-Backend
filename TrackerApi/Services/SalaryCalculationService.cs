using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NodaTime;
using TrackerApi.ApiModels;
using TrackerApi.Extensions;
using TrackerApi.Extensions.ApiModels;
using TrackerApi.Extensions.Models;
using TrackerApi.Models.Enums;
using TrackerApi.Services.Interfaces;

namespace TrackerApi.Services
{
    public class SalaryCalculationService : ISalaryCalculationService
    {
        private readonly IActivityService _activityService;
        private readonly IHolidayService _holidayService;
        private readonly IAbsenceService _absenceService;
        private readonly IUserRateService _rateService;
        private readonly IProjectService _projectService;
        public SalaryCalculationService(IActivityService activityService, IHolidayService holidayService, IAbsenceService absenceService, IUserRateService rateService, IProjectService projectService)
        {
            _activityService = activityService;
            _holidayService = holidayService;
            _absenceService = absenceService;
            _rateService = rateService;
            _projectService = projectService;
        }

        public async Task<MonthSalaryApiModel> GetSalaryByUserIdMonth(int userId, int month, int year)
        {
            var kievZone = DateTimeZoneProviders.Tzdb["Europe/Kiev"];

            var startOfMonth = kievZone.AtStartOfDay(new LocalDate(year, month, 1));
            var calendar = startOfMonth.Calendar;
            var endOfMonth =
                startOfMonth.LocalDateTime.PlusDays(calendar.GetDaysInMonth(startOfMonth.Year, startOfMonth.Month)).PlusMilliseconds(-1);//new LocalDateTime(startOfMonth.Year, startOfMonth.Month, calendar.GetDaysInMonth(startOfMonth.Year, startOfMonth.Month), 23, 59, 59);

            var start = startOfMonth.ToDateTimeUtc();
            var end = kievZone.AtStrictly(endOfMonth).ToDateTimeUtc();

            var activityFilterApiModel = new ActivityFilterApiModel()
            {
                StartDate = start,
                EndDate = end
            };

            var result = new MonthSalaryApiModel
            {
                Month = month,
                Year = year,
                ActivitySalaries = await GetCalculatedActivities(userId, activityFilterApiModel),
                PublicHolidaySalaries = await GetCalculatedHolidays(userId, start, end),
                AbsenceSalaries = await GetCalculatedAbsences(userId, start, end)
            };

            result.MonthAmount = result.ActivitySalaries.Sum(x => x.Amount) + result.AbsenceSalaries.Sum(x => x.Amount) + result.PublicHolidaySalaries.Sum(x => x.Amount);

            return result;
        }

        private async Task<List<AbsenceSalary>> GetCalculatedAbsences(int userId, DateTime start, DateTime end)
        {
            var result = new List<AbsenceSalary>();

            var absences = await _absenceService.GetApprovedAbsencesByDates(start, end, userId);

            foreach (var absence in absences)
            {
                var endDate = absence.EndDate;
                if (endDate > end)
                {
                    endDate = end;
                }

                var startDate = absence.StartDate;
                if (startDate < start)
                {
                    startDate = start;
                }

                var startAbsenceString = startDate.UtcToZonedDateTime("Europe/Kiev").ToDateTimeUnspecified()
                    .ToShortDateString();
                var endAbsenceString = endDate.UtcToZonedDateTime("Europe/Kiev").ToDateTimeUnspecified()
                    .ToShortDateString();

                var ab = new AbsenceSalary
                {
                    HoursCount = Math.Round((decimal)(absence.GetAbsenceTotalDays(start, end) * 8), 2),
                    Type = absence.Type,
                    Dates = startAbsenceString == endAbsenceString ? startAbsenceString : $"{startAbsenceString} - {endAbsenceString}"
                };
                switch (ab.Type)
                {
                    case AbsenceType.SickLeave:
                        {
                            var rateData = await _rateService.GetRatesByDateUserId(userId, absence.StartDate);
                            ab.RatePerHour = rateData.Rate;
                            ab.Amount = Math.Round(ab.HoursCount * ab.RatePerHour, 2);
                            break;
                        }
                    case AbsenceType.Vacation:
                        {
                            var userRates = (await _rateService.GetRatesByUserId(userId)).ToArray();
                            if (userRates.Any())
                            {
                                var oldest = userRates.Last();

                                var monthDiff = oldest.Date.GetMonthDiff(absence.StartDate);
                                var ratesList = new List<RateDate>();

                                var monthLimit = monthDiff >= 12 ? 12 : monthDiff;

                                var date = absence.StartDate;
                                var count = 0;
                                while (count < monthLimit)
                                {
                                    var closestRate = userRates.Where(d => (d.Date - date).Ticks < 0)
                                        .OrderByDescending(d => (d.Date - date).Ticks).First();

                                    var currentMonthRates = userRates.Where(x => x.Date.Month == closestRate.Date.Month).ToArray();

                                    var currentRate = currentMonthRates.Length > 1
                                        ? currentMonthRates.Average(x => x.Rate)
                                        : closestRate.Rate;

                                    ratesList.Add(new RateDate()
                                    {
                                        Date = date.ToShortDateString(),
                                        Rate = currentRate
                                    });
                                    date = date.AddMonths(-1);
                                    count++;
                                }

                                ab.RatePerHour = ratesList.Average(x => x.Rate);
                                ab.Amount = Math.Round(ab.HoursCount * ab.RatePerHour);
                            }

                            break;
                        }
                }

                result.Add(ab);
            }

            return result;
        }

        private async Task<List<PublicHolidaySalary>> GetCalculatedHolidays(int userId, DateTime start, DateTime end)
        {
            var result = new List<PublicHolidaySalary>();

            var holidays = await _holidayService.GetHolidaysByYears(start, end);
            foreach (var holiday in holidays)
            {
                var rate = await _rateService.GetRatesByDateUserId(userId, holiday.Date);
                var holidaySalary = new PublicHolidaySalary
                {
                    Holiday = holiday.Description,
                    RatePerHour = rate.Rate,
                    HoursCount = 8
                };
                holidaySalary.Amount = Math.Round(holidaySalary.HoursCount * holidaySalary.RatePerHour, 2);
                result.Add(holidaySalary);
            }

            return result;
        }

        private async Task<ICollection<ActivitySalary>> GetCalculatedActivities(int userId, ActivityFilterApiModel activityFilterApiModel)
        {
            var result = new List<ActivitySalary>();

            var activities = await _activityService.GetAllByFilterAsync(activityFilterApiModel, new[] { userId });

            foreach (var activity in activities.GroupBy(x => x.ProjectId))
            {
                var activitiesByProject = activity.ToArray();
                var rateActivities = new Dictionary<decimal, ICollection<ActivityApiModel>>();

                var project = await _projectService.GetByIdAsync(activity.Key);

                foreach (var activityByProject in activitiesByProject)
                {
                    var rate = await _rateService.GetRatesByDateUserId(userId, activityByProject.WorkedFromUtc);

                    if (rate != null)
                    {
                        if (!rateActivities.ContainsKey(rate.Rate))
                        {
                            rateActivities.Add(rate.Rate, new List<ActivityApiModel>());
                        }

                        rateActivities[rate.Rate].Add(activityByProject);
                    }
                }

                if (!rateActivities.Any()) continue;
                foreach (var uniqueRate in rateActivities.Keys)
                {
                    var activitiesByRate = rateActivities[uniqueRate];

                    var activitySalary = new ActivitySalary { Project = project.Name };
                    var hoursCount = Math.Round(activitiesByRate.Sum(x => x.Duration / 60), 2);
                    activitySalary.HoursCount = hoursCount;
                    activitySalary.RatePerHour = uniqueRate * (decimal)(project.MainCof <= 0 ? 1 : project.MainCof);
                    activitySalary.Amount = Math.Round(activitySalary.HoursCount * activitySalary.RatePerHour, 2);
                    result.Add(activitySalary);
                }
            }

            return result;
        }
    }
}