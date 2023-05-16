using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrackerApi.ApiModels;
using TrackerApi.Models;
using TrackerApi.Repositories.Interfaces;
using TrackerApi.Services.Interfaces;
using System.Linq;
using TrackerApi.Extensions.Models;
using Microsoft.AspNetCore.Identity;
using System;
using TrackerApi.Models.Enums;

namespace TrackerApi.Services
{
    public class NotificationService : BaseService<NotificationApiModel, Notification>, INotificationService
    {
        INotificationRepository _notificationRepository;
        private readonly UserManager<IdentityAuthUser> _userManager;


        public NotificationService(INotificationRepository notificationRepository, UserManager<IdentityAuthUser> userManager, IMapper mapper) : base(notificationRepository, mapper)
        {
            _notificationRepository = notificationRepository;
            _userManager = userManager;
        }

        public async Task<IEnumerable<NotificationApiModel>> GetAllUserNotifications(int userId)
        {
            var notifications =  await _notificationRepository.GetAllAsync(x => x.UserId == userId);
            return _mapper.Map<IEnumerable<NotificationApiModel>>(notifications);
        }

        public async Task<PagedResult<NotificationApiModel>> GetNotificationPage(NotificationFilterApiModel filter)
        {
            return await _notificationRepository
                .GetPageAsync<NotificationApiModel>(x => x.UserId == filter.UserId, filter.Page, filter.PageSize, n => n.CreatedOn);
        }

        public async Task<NotificationAlertResponse> GetUserAlertNotifications(int userId, int notificationAmount)
        {
            var notifications = await _notificationRepository.GetAllAsync(x => x.UserId == userId);

            return new NotificationAlertResponse()
            {
                TotalAmount = notifications.Count(),
                Notifications = _mapper.Map<IEnumerable<NotificationApiModel>>(notifications.Take(notificationAmount))
            };
        }

        public async Task<bool> NotifyAllAbsenceApprovers(AbsenceApiModel createdAbsence)
        {
            var absenceApprovers = await _userManager.GetUsersInRoleAsync("AbsenceApprover");

            var notifictions = CreateUserNotifications(
                absenceApprovers,
                "New Absence Request",
                $"You have new { createdAbsence.Type } absence to approve!");

            await _notificationRepository.CreateRangeAsync(notifictions);

            return true;
        }


        public async Task<bool> AbsenceUpdateNotification(AbsenceApiModel updatedAbsence)
        {
            await _notificationRepository.CreateAsync(new Notification() { 
                Caption = "Absence Request progress",
                Description = $"Your request has been {Enum.GetName(typeof(AbsenceStatus), updatedAbsence.Type)}",
                CreatedOn = DateTime.UtcNow,
                IsRead = false,
                UserId = updatedAbsence.UserId
            });

            return true;
        }

        private List<Notification> CreateUserNotifications(IList<IdentityAuthUser> users, string caption, string description)
        {
            List<Notification> notifications = new List<Notification>();

            foreach (var user in users)
            {
                notifications.Add(new Notification()
                {
                    Caption = caption,
                    Description = description,
                    IsRead = false,
                    CreatedOn = DateTime.UtcNow,
                    UserId = user.Id
                });
            }

            return notifications;
        }
    }
}
