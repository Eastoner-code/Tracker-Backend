using CsvHelper.Configuration.Attributes;
using System;

namespace TrackerApi.ApiModels.CSVModels
{
    public class ActivityCsvModel
    {
        [Name("activity")]
        public string Description { get; set; }
        [Name("project")]
        public string Project { get; set; }
        [Name("workers")]
        public string UserName { get; set; }
        [Name("duration_seconds")]
        public string Duration { get; set; }
        [Name("start_time")]
        public DateTime WorkedFrom { get; set; }
        [Name("end_time")]
        public DateTime WorkedTo { get; set; }
    }
}
