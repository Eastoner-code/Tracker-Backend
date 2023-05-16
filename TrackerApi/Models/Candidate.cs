using System;
using System.ComponentModel.DataAnnotations.Schema;
using TrackerApi.Models.Enums;
using TrackerApi.Models.Interfaces;

namespace TrackerApi.Models
{
    public class Candidate : IBaseModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public CandidateStatus Status { get; set; }

        public string Description { get; set; }

        public DateTime InterviewDate { get; set; }
        
        public int VacancyId { get; set; }

        [ForeignKey("VacancyId")]
        public Vacancy Vacancy { get; set; }
    }
}
