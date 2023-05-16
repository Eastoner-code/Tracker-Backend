using System;

namespace TrackerApi.ApiModels
{
    public class UserRateApiModel
    {
        public int Id { get; set; }
        public decimal Rate { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
    }
}