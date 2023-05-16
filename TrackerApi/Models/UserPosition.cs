using System;
using System.Collections.Generic;

namespace TrackerApi.Models
{
    public partial class UserPosition
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PositionId { get; set; }

        public virtual IdentityAuthUser User { get; set; }
        public virtual Position Position { get; set; }
    }
}
