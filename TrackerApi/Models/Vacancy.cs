using System;
using TrackerApi.Models.Interfaces;

namespace TrackerApi.Models
{
    public class Vacancy : IBaseModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime DateOpened { get; set; }

        public DateTime DueDate { get; set; }

        public bool isOpened { get; set; }
    }
}
