using System.Collections.Generic;

namespace TrackerApi.ApiModels
{
    public class NotificationAlertResponse
    {
        public IEnumerable<NotificationApiModel> Notifications { get; set; }

        public int TotalAmount { get; set; }
    }
}
