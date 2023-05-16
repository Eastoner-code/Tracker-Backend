using System;
using System.Collections.Generic;
using TrackerApi.Models.Interfaces;

namespace TrackerApi.Models
{
    public class InvoicePipeline : IBaseModel
    {
        public InvoicePipeline()
        {
            InvoiceUserProject = new HashSet<InvoiceUserProject>();
            PaymentDetails = new HashSet<PaymentDetails>();
        }
        public int Id { get; set; }
        public string InvoiceNumber { get; set; }
        public ICollection<InvoiceUserProject> InvoiceUserProject { get; set; }
        public ICollection<PaymentDetails> PaymentDetails { get; set; }
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAdressUA { get; set; }
        public string CustomerAdressENG { get; set; }
        public string CustomerEmail { get; set; }
        public int? SubTotal { get; set; }
        public int? Total { get; set; }
        public bool IsSent { get; set; }
        public int UserId { get; set; }
        public virtual IdentityAuthUser User { get; set; }

    }
}
