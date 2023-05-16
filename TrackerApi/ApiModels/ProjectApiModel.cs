using System;
using System.Collections.Generic;
using TrackerApi.Models;

namespace TrackerApi.ApiModels
{
    public class ProjectApiModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double MainCof { get; set; } = 1;
        public double WeekCof { get; set; } = 1;
        public double OverCof { get; set; } = 1;
        public bool isArchive { get; set; } = false;
        public DateTime? CreatedAtUtc { get; set; }
        public DateTime? UpdatedAtUtc { get; set; }
        public string Meta { get; set; }
        public Guid CustomerUrl { get; set; } = Guid.NewGuid();

        //public ICollection<Activity> Activity { get; set; }
    }
}