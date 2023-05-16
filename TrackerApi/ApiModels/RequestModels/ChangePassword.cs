namespace TrackerApi.ApiModels.RequestModels
{
    public class ChangePassword
    {
        public string Email { get; set; }
        public string CurrentPasword { get; set; }
        public string NewPassword { get; set; }
}
}
