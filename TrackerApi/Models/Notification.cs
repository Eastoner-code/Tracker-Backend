using System;
using System.ComponentModel.DataAnnotations.Schema;
using TrackerApi.Models.Interfaces;

namespace TrackerApi.Models
{
    public class Notification : IBaseModel
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Caption { get; set; }

        public string Description { get; set; }

        public bool IsRead { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual IdentityAuthUser User { get; set; }
    }
}
