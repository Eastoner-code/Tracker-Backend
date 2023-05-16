using AutoMapper;
using Microsoft.Extensions.Logging;
using TrackerApi.Models;
using TrackerApi.Repositories.Interfaces;

namespace TrackerApi.Repositories
{
    public class NotificationRepostory : BaseRepository<Notification>, INotificationRepository
    {
        public NotificationRepostory(TrackerContext trackerContext, ILogger<BaseRepository<Notification>> logger, IMapper mapper) : base(trackerContext, logger, mapper) {}
    }
}
