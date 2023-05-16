using System;

namespace TrackerApi.ApiModels
{
    public class NotificationApiModel
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Caption { get; set; }

        public string Description { get; set; }

        public bool IsRead { get; set; }

        public int UserId { get; set; }
    }
}
