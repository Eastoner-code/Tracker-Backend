using System;
using System.Collections.Generic;

namespace TrackerApi.ApiModels
{
    public class InvoicePipelineApiModel
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAdressUA { get; set; }
        public string CustomerAdressENG { get; set; }
        public string CustomerEmail { get; set; }
        public int UserId { get; set; }
        public List<InvoiceProjectApiModel> Projects { get; set; }
    }
}
