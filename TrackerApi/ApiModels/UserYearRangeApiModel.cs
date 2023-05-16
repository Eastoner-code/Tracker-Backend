using System;

namespace TrackerApi.ApiModels
{
    public class UserYearRangeApiModel
    {
        public int Id { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}