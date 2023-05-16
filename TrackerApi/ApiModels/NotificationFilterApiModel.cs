namespace TrackerApi.ApiModels
{
    public class NotificationFilterApiModel : IBasePageFilter
    {
        public int Page { get; set; }
        public int PageSize { get; set; }

        public int UserId { get; set; }
    }
}
