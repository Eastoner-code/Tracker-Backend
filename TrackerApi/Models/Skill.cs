using System;
using System.Collections.Generic;
using TrackerApi.Models.Interfaces;

namespace TrackerApi.Models
{
    public partial class Skill : IBaseModel
    {
        public Skill()
        {
            UserSkill = new HashSet<UserSkill>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<UserSkill> UserSkill { get; set; }
    }
}
