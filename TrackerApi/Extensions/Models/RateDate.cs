namespace TrackerApi.Extensions.Models
{
    public class RateDate
    {
        public decimal Rate { get; set; }
        public string Date { get; set; }

        public override string ToString()
        {
            return $"{Rate} - {Date}";
        }
    }
}
