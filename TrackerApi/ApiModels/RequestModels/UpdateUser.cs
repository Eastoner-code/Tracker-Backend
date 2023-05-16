using System;

namespace TrackerApi.ApiModels.RequestModels
{
    public class UpdateUser
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string LastName { get; set; }
        public string TimeZone { get; set; }
        public DateTime? StartDateUtc { get; set; }
        public string Meta { get; set; } = "{}";
    }
}