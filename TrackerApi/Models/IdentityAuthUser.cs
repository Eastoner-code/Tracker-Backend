using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TrackerApi.Models.Interfaces;

namespace TrackerApi.Models
{
    public class IdentityAuthUser : IdentityUser<int>, IBaseModel
    {
        public IdentityAuthUser()
        {
            Activity = new HashSet<Activity>();
            UserPosition = new HashSet<UserPosition>();
            UserProject = new HashSet<UserProject>();
            UserSkill = new HashSet<UserSkill>();
            InvoiceUserProject = new HashSet<InvoiceUserProject>();
            InvoicePipeline = new HashSet<InvoicePipeline>();
        }

        public override int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? StartDateUtc { get; set; }
        public string TimeZone { get; set; }
        public string Meta { get; set; }
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        public virtual ICollection<Activity> Activity { get; set; }
        public virtual ICollection<UserPosition> UserPosition { get; set; }
        public virtual ICollection<UserProject> UserProject { get; set; }
        public virtual ICollection<UserSkill> UserSkill { get; set; }
        public virtual ICollection<InvoiceUserProject> InvoiceUserProject { get; set; }
        public virtual ICollection<InvoicePipeline> InvoicePipeline { get; set; } 
    }
}
