using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TrackerApi.Models.Interfaces;

namespace TrackerApi.Models
{
    public class UserRate : IBaseModel
    {
        public int Id { get; set; }
        public decimal Rate { get; set; }
        public DateTime Date { get; set; }
        [ForeignKey("UserId")]
        public IdentityAuthUser User { get; set; }
        public int UserId { get; set; }
    }
}