using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackerApi.Models.HelperModels
{
    public class GeneralCustomerReport
    {
        public string ProjectName { get; set; }
        public UserCustomerReport[] UserReports { get; set; }
        public TimeCustomerReport TotalTime { get; set; }
        public ActivityCustomerReport[] Activities { get; set; }
    }

    public class ActivityCustomerReport
    {
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public decimal? Duration { get; set; }
    }

    public class UserCustomerReport
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public TimeCustomerReportForPeriod[] ReportForWeeks { get; set; }
        public TimeCustomerReport TotalTime { get; set; }
    }

    public class TimeCustomerReport
    {
        public int Hours { get; set; } = 0;

        public int Minutes { get; set; } = 0;
    }

    public class TimeCustomerReportForPeriod : TimeCustomerReport
    {
        public DateTime From  { get; set; }

        public DateTime To { get; set; }
    }
}
