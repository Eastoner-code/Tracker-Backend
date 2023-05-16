using System;
using TrackerApi.Models.Enums;

namespace TrackerApi.ApiModels
{
    public class CandidateApiModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public CandidateStatus Status { get; set; }

        public string Description { get; set; }

        public DateTime InterviewDate { get; set; }

        public int VacancyId { get; set; }
    }
}
