using System.Collections.Generic;

namespace TrackerApi.ApiModels
{
    public class InvoiceProjectApiModel
    {
        public int ProjectId { get; set; }
        public List<InvoiceDeveloperApiModel> Developers { get; set; }
    }
}
