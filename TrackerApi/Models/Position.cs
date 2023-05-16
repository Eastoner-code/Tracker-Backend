using System;
using System.Collections.Generic;
using TrackerApi.Models.Interfaces;

namespace TrackerApi.Models
{
    public partial class Position : IBaseModel
    {
        public Position()
        {
            UserPosition = new HashSet<UserPosition>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<UserPosition> UserPosition { get; set; }
    }
}
