using System;
using System.Collections.Generic;

namespace TrackerApi.Models
{
    public partial class UserSkill
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SkillId { get; set; }

        public virtual IdentityAuthUser User { get; set; }
        public virtual Skill Skill { get; set; }
    }
}
