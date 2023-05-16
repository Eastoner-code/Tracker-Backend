using TrackerApi.Models.Interfaces;

namespace TrackerApi.Models
{
    public class InvoiceUserProject : IBaseModel
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int UserId { get; set; }
        public int ProjectId { get; set; }
        public decimal Rate { get; set; }
        public virtual InvoicePipeline Invoice { get; set; }
        public virtual IdentityAuthUser User { get; set; }
        public virtual Project Project { get; set; }

    }
}
