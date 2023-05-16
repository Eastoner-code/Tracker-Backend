using System;
using System.Collections.Generic;
using TrackerApi.Models.Interfaces;

namespace TrackerApi.Models
{
    public partial class Project : IBaseModel
    {
        public Project()
        {
            Activity = new HashSet<Activity>();
            UserProject = new HashSet<UserProject>();
            InvoicePipeline = new HashSet<InvoicePipeline>();
            InvoiceUserProject = new HashSet<InvoiceUserProject>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime UpdatedAtUtc { get; set; }
        public double MainCof { get; set; } = 1;
        public double WeekCof { get; set; } = 1;
        public double OverCof { get; set; } = 1; 
        public string Meta { get; set; }
        public bool IsArchive { get; set; } = false; 
        public Guid CustomerUrl { get; set; } = new Guid();

        public virtual ICollection<Activity> Activity { get; set; }
        public virtual ICollection<UserProject> UserProject { get; set; }
        public virtual ICollection<InvoicePipeline> InvoicePipeline { get; set; }
        public virtual ICollection<InvoiceUserProject> InvoiceUserProject { get; set;}
    }
}
