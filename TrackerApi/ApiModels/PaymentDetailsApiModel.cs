namespace TrackerApi.ApiModels
{
    public class PaymentDetailsApiModel
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public string ContactPersonNameENG { get; set; }
        public string ContactPersonNameUA { get; set; }
        public string AdressENG { get; set; }
        public string AdressUA { get; set; }
        public string Details { get; set; }
    }
}
