using System;
using System.ComponentModel.DataAnnotations.Schema;
using TrackerApi.Models.Enums;
using TrackerApi.Models.Interfaces;

namespace TrackerApi.Models
{
    public partial class Absence: IBaseModel
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartDateLocal { get; set; }
        public DateTime EndDateLocal { get; set; }
        public bool IsFullDay { get; set; }
        public AbsenceType Type { get; set; }
        public AbsenceStatus Status { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public IdentityAuthUser User { get; set; }
        public int? ApprovedByUserId { get; set; }
        [ForeignKey("ApprovedByUserId")]
        public IdentityAuthUser ApprovedByUser { get; set; }
        public string Description { get; set; }
        public int UserYearRangeId { get; set; }
        [ForeignKey("UserYearRangeId")]
        public UserYearRange UserYearRange { get; set; }
        [NotMapped] public string UserName => this.User?.FullName;
    }
}