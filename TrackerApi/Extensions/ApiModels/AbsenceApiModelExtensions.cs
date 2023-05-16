using System;
using NodaTime;
using TrackerApi.ApiModels;

namespace TrackerApi.Extensions.ApiModels
{
    public static class AbsenceApiModelExtensions
    {
        public static double GetAbsenceTotalDays(this AbsenceApiModel absence, DateTime? start = null, DateTime? end = null)
        {
            if (absence.IsFullDay)
            {
                var endDate = absence.EndDate;
                var startDate = absence.StartDate;
                if (end != null && start != null)
                {
                    if (endDate > end)
                    {
                        endDate = end.Value;
                    }

                    if (startDate < start)
                    {
                        startDate = start.Value;
                    }
                }

                var total = Math.Round((endDate - startDate).TotalDays, 0);

                var startUserTime = startDate.UtcToZonedDateTime("Europe/Kiev");
                var endUserTime = endDate.UtcToZonedDateTime("Europe/Kiev");

                var weekends = 0;
                var from = startUserTime.ToDateTimeUnspecified();

                while (from <= endUserTime.ToDateTimeUnspecified())
                {
                    if (@from.IsWeekend())
                    {
                        weekends += 1;
                    }

                    from = from.AddDays(1);
                }

                return total - weekends;
            }
            else
            {
                if (absence.StartDateLocal.IsWeekend())
                {
                    return 0;
                }
                else
                {
                    return 0.5;
                }
            }
        }

        public static bool IsWeekend(this DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }

        public static int GetMonthDiff(this DateTime @from, DateTime to)
        {
            var diff = 0;
            var currentFrom = from.AddMonths(1);
            while (currentFrom < to)
            {
                currentFrom = currentFrom.AddMonths(1);
                diff++;
            }

            return diff;
        }
    }
}