using System;
using System.Collections.Generic;

namespace TrackerApi.ApiModels
{
    public class SalariesApiModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public decimal Salary { get; set; }
        public decimal Payed { get; set; }
        public ICollection<SalaryPayedApiModel> Operations { get; set; }
    }

    public class SalaryPayedApiModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
    }
}