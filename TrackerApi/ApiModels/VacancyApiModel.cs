using System;

namespace TrackerApi.ApiModels
{
    public class VacancyApiModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime DateOpened { get; set; }

        public DateTime DueDate { get; set; }

        public bool isOpened { get; set; }
    }
}
