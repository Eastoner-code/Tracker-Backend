using System;

namespace TrackerApi.ApiModels.RequestModels
{
    public class RegisterUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime? StartDateUtc { get; set; }
        public string Meta { get; set; }
    }
}