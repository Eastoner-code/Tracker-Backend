namespace TrackerApi.ApiModels
{
    public class UserYearRangeFullApiModel : UserYearRangeApiModel
    {
        public double AvailableVacations { get; set; }
        public double AvailableSickLeaves { get; set; }

        public double UsedVacations { get; set; }
        public double UsedSickLeaves { get; set; }
        public double UsedDayOffs { get; set; }
    }
}