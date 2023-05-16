using System;
using System.Collections.Generic;
using System.Threading;
using NodaTime;
using TrackerApi.Extensions.Models;

// ReSharper disable All

namespace TrackerApi.Extensions
{
    public static partial class DateTimeExtensions
    {
        public static DateTime FirstDayOfWeek(this DateTime dt)
        {
            var culture = Thread.CurrentThread.CurrentCulture;
            var diff = dt.DayOfWeek - culture.DateTimeFormat.FirstDayOfWeek;

            if (diff < 0)
            {
                diff += 7;
            }

            return dt.AddDays(-diff).Date;
        }

        public static DateTime LastDayOfWeek(this DateTime dt) =>
            dt.FirstDayOfWeek().AddDays(6);

        public static DateTime FirstDayOfMonth(this DateTime dt) =>
            new DateTime(dt.Year, dt.Month, 1);

        public static DateTime LastDayOfMonth(this DateTime dt) =>
            dt.FirstDayOfMonth().AddMonths(1).AddDays(-1);

        public static DateTime FirstDayOfthePreviousMonth(this DateTime dt) =>
            dt.FirstDayOfMonth().AddMonths(-1);

        public static DateTime FirstDayOfNextMonth(this DateTime dt) =>
            dt.FirstDayOfMonth().AddMonths(1);

        public static List<WeekItem> GetMonthWeekList(this DateTime dt)
        {
            var startDate = dt.FirstDayOfMonth();
            var endDate = dt.LastDayOfMonth();
            var items = new List<WeekItem>();
            int diff = (7 + (startDate.DayOfWeek - DayOfWeek.Monday)) % 7;
            var weekStartDate = startDate.AddDays(-1 * diff).Date;
            var weekEndDate = DateTime.MinValue;
            while (weekEndDate < endDate)
            {
                weekEndDate = weekStartDate.AddDays(6);
                var shownStartDate = weekStartDate < startDate ? startDate : weekStartDate;
                var shownEndDate = weekEndDate > endDate ? endDate : weekEndDate;

                items.Add(new WeekItem { From = shownStartDate,
                To = shownEndDate
                });
                weekStartDate = weekStartDate.AddDays(7);
            }
            return items;
        }

        public static ZonedDateTime UtcToZonedDateTime(this DateTime dateTime, string zone)
        {
            var kievZone = DateTimeZoneProviders.Tzdb[zone];
            var instant = Instant.FromDateTimeUtc(DateTime.SpecifyKind(dateTime, DateTimeKind.Utc));
            
            return instant.InZone(kievZone);
        }
        public static DateTime StartDateForActivityNotification(this DateTime today)
        {
            DateTime startDate;
            if (today.Date != today.FirstDayOfMonth())
            {
                switch (today.DayOfWeek)
                {
                    case DayOfWeek.Monday: startDate = today.AddMonths(-1).FirstDayOfMonth(); break;
                    case DayOfWeek.Friday: startDate = today.AddDays(-4); break;
                    default: return today;
                }
            }
            else startDate = today.AddMonths(-1).FirstDayOfMonth();
            return startDate.Date;
        }

        public static LocalDateTime ConvertToLocalDateTimeWithoutTime(this DateTime dt,DateTimeZone timeZone)
        {
            return Instant.FromDateTimeUtc(DateTime.SpecifyKind(dt, DateTimeKind.Utc)).InZone(timeZone).Date.AtStartOfDayInZone(timeZone).LocalDateTime;
        }
        public static LocalDateTime ConvertToLocalDateTime(this DateTime dt, DateTimeZone timeZone)
        {
            return Instant.FromDateTimeUtc(DateTime.SpecifyKind(dt, DateTimeKind.Utc)).InZone(timeZone).LocalDateTime;
        }
    }

}