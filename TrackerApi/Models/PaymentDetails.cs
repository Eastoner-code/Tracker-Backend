using System;
using System.Collections.Generic;
using TrackerApi.Models.Interfaces;

namespace TrackerApi.Models
{
    public class PaymentDetails : IBaseModel
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public string ContactPersonNameENG { get; set; }
        public string ContactPersonNameUA { get; set; }
        public string AdressENG { get; set; }
        public string AdressUA { get; set; }
        public string Details { get; set; }
        public virtual InvoicePipeline Invoice { get; set; }
    }
}
