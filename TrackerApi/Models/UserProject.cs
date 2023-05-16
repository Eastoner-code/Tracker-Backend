using System;
using System.Collections.Generic;

namespace TrackerApi.Models
{
    public partial class UserProject
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProjectId { get; set; }

        public virtual IdentityAuthUser User { get; set; }
        public virtual Project Project { get; set; }
    }
}
