namespace TrackerApi.ApiModels
{
    public class UserReport
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public decimal TotalHours { get; set; }
        public decimal CurrentWeekHours { get; set; }
        public decimal CurrentMonthHours { get; set; }
        public decimal PreviousMonthHours { get; set; }
    }
}