using System;
using System.Collections.Generic;

namespace TrackerApi.ApiModels
{
    public class UserApiModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? CreatedAtUtc { get; set; }
        public DateTime? UpdatedAtUtc { get; set; }
        public DateTime? StartDateUtc { get; set; }
        public string Meta { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string TimeZone { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public int UserId { get; set; }




        //public virtual ICollection<Activity> Activity { get; set; }
        //public virtual ICollection<UserPosition> UserPosition { get; set; }
        //public virtual ICollection<UserProject> UserProject { get; set; }
        //public virtual ICollection<UserRole> UserRole { get; set; }
        //public virtual ICollection<UserSkill> UserSkill { get; set; }
    }
}