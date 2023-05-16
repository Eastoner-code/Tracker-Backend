using System.Collections.Generic;
using TrackerApi.Models.Enums;

namespace TrackerApi.ApiModels
{
    public class MonthSalaryApiModel
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal MonthAmount { get; set; }
        public ICollection<AbsenceSalary> AbsenceSalaries { get; set; } = new List<AbsenceSalary>();
        public ICollection<ActivitySalary> ActivitySalaries { get; set; } = new List<ActivitySalary>();
        public ICollection<PublicHolidaySalary> PublicHolidaySalaries { get; set; } = new List<PublicHolidaySalary>();
    }

    public class AbsenceSalary
    {
        public AbsenceType Type { get; set; }
        public string Dates { get; set; }
        public decimal HoursCount { get; set; }
        public decimal RatePerHour { get; set; }
        public decimal Amount { get; set; }
    }

    public class ActivitySalary
    {
        public string Project { get; set; }
        public decimal HoursCount { get; set; }
        public decimal RatePerHour { get; set; }
        public decimal Amount { get; set; }
    }

    public class PublicHolidaySalary
    {
        public string Holiday { get; set; }
        public int HoursCount { get; set; }
        public decimal RatePerHour { get; set; }
        public decimal Amount { get; set; }
    }
}