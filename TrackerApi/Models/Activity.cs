using System;
using System.Collections.Generic;
using TrackerApi.Models.Interfaces;

namespace TrackerApi.Models
{
    public partial class Activity: IBaseModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime UpdatedAtUtc { get; set; }
        public DateTime WorkedFromUtc { get; set; }
        public DateTime WorkedToUtc { get; set; }
        public decimal? Duration { get; set; }
        public int ProjectId { get; set; }
        public int UserId { get; set; }

        public virtual IdentityAuthUser User { get; set; }
        public virtual Project Project { get; set; }
    }
}
