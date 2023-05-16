using System;
using TrackerApi.Models.Interfaces;

namespace TrackerApi.Models
{
    public class Holiday : IBaseModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}