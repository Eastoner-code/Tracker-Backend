using System;

namespace TrackerApi.ApiModels
{
    public class HolidayApiModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}