using System;
using TrackerApi.Extensions.ApiModels;
using TrackerApi.Models.Enums;

namespace TrackerApi.ApiModels
{
    public class AbsenceApiModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartDateLocal { get; set; }
        public DateTime EndDateLocal { get; set; }
        public bool IsFullDay { get; set; }
        public AbsenceType Type { get; set; }
        public AbsenceStatus Status { get; set; }
        public int UserId { get; set; }
        public int? ApprovedByUserId { get; set; }
        public string Description { get; set; }
        public double TotalDays => this.GetAbsenceTotalDays();
    }
}