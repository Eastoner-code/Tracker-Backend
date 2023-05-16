using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TrackerApi.Models.Interfaces;

namespace TrackerApi.Models
{
    public class UserYearRange : IBaseModel
    {
        public int Id { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public double VacationCount { get; set; }
        public double SickLeaveCount { get; set; }
        public int CalculatedMonth { get; set; }
        [ForeignKey("UserId")]
        public IdentityAuthUser User { get; set; }
        public int UserId { get; set; }
        public bool IsVacationsTransferredToNextRange { get; set; } = false;
        public virtual ICollection<Absence> Absences { get; set; } = new List<Absence>();
    }
}