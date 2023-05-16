using System.Collections.Generic;
using System.Threading.Tasks;
using TrackerApi.ApiModels;
using TrackerApi.Extensions.Models;

namespace TrackerApi.Services.Interfaces
{
    public interface INotificationService : IBaseService<NotificationApiModel> 
    {
        Task<IEnumerable<NotificationApiModel>> GetAllUserNotifications(int userId);

        Task<NotificationAlertResponse> GetUserAlertNotifications(int userId, int notificationsAmount);

        Task<PagedResult<NotificationApiModel>> GetNotificationPage(NotificationFilterApiModel filter);

        Task<bool> NotifyAllAbsenceApprovers(AbsenceApiModel absence);

        Task<bool> AbsenceUpdateNotification(AbsenceApiModel absence);
    }
}
