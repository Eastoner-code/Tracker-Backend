using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackerApi.ApiModels
{
    public class GeneralCustomerReportApiModal
    {
        public string ProjectName { get; set; }
        public UserCustomerReportApiModal[] UserReports { get; set; }
        public TimeCustomerReportApiModal TotalTime { get; set; }
        public ActivityCustomerReportApiModal[] Activities { get; set; }
    }

    public class ActivityCustomerReportApiModal
    {
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public decimal? Duration { get; set; }
    }

    public class UserCustomerReportApiModal
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public TimeCustomerReportForPeriodApiModal[] ReportForWeeks { get; set; }
        public TimeCustomerReportApiModal TotalTime { get; set; }
    }

    public class TimeCustomerReportApiModal
    {
        public int Hours { get; set; }

        public int Minutes { get; set; }
    }

    public class TimeCustomerReportForPeriodApiModal : TimeCustomerReportApiModal
    {
        public DateTime From { get; set; }

        public DateTime To { get; set; }
    }
}
