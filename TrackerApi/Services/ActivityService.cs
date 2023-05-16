using AutoMapper;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackerApi.ApiModels;
using TrackerApi.Extensions;
using TrackerApi.Extensions.Models;
using TrackerApi.Models;
using TrackerApi.Models.Enums;
using TrackerApi.Repositories.Interfaces;
using TrackerApi.Services.Interfaces;

namespace TrackerApi.Services
{
    public class ActivityService : BaseService<ActivityApiModel, Activity>, IActivityService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IAbsenceRepository _absenceRepository;
        private readonly IUserRepository _userRepository;
        private readonly IInvoicePipelineRepository _invoicePipelineRepository;
        public ActivityService(IActivityRepository repository, IMapper mapper, INotificationRepository notificationRepository, IAbsenceRepository absenceRepository, IUserRepository userRepository, IInvoicePipelineRepository invoicePipelineRepository) : base(repository, mapper)
        {
            _notificationRepository = notificationRepository;
            _absenceRepository = absenceRepository;
            _userRepository = userRepository;
            _invoicePipelineRepository = invoicePipelineRepository;
        }

        public async Task<Task> CheckUserTrackings(int userId)
        {
            var userTimeZone = (await _userRepository.GetByAsync(u => u.Id == userId)).TimeZone;
            var timeZone = DateTimeZoneProviders.Tzdb[userTimeZone];

            var endDateUtc = DateTime.UtcNow;
            var endDateLocal = endDateUtc.ConvertToLocalDateTimeWithoutTime(timeZone);

            var startDateUtc = endDateUtc.StartDateForActivityNotification();
            var startDateLocal = startDateUtc.ConvertToLocalDateTimeWithoutTime(timeZone);
            
            if (startDateLocal.Date == endDateLocal.Date) return Task.CompletedTask;
            
            var activities = await GetAllByFilterTrackedAsync(startDateUtc, endDateUtc, userId);
            var absenses = await _absenceRepository.GetAllAsync(abs => abs.StartDate.ConvertToLocalDateTime(timeZone) >= startDateLocal && abs.EndDate.ConvertToLocalDateTime(timeZone) <= endDateLocal.PlusDays(1) && abs.Status == AbsenceStatus.Approved);
            int noTrackedDays = 0;

            if(activities.Count == 0 && absenses.Count == 0)
            {
                await CreateNotificationNothingTracked(userId, startDateLocal.Date);
                return Task.CompletedTask;
            }

            for (LocalDateTime day = startDateLocal; day.Date < endDateLocal.Date; day = day.PlusHours(24))
            {
                if (day.DayOfWeek != IsoDayOfWeek.Saturday && day.DayOfWeek != IsoDayOfWeek.Sunday)
                {
                    var trackedActivitiesByDay = activities.FindAll(x => (x.WorkedFromUtc.ConvertToLocalDateTime(timeZone) >= day && x.WorkedToUtc.ConvertToLocalDateTime(timeZone) <= day.PlusHours(24)));
                    var trackedAbsensesByDay = absenses.FindAll(abs => (day >= abs.StartDate.ConvertToLocalDateTime(timeZone) && day <= abs.EndDate.ConvertToLocalDateTime(timeZone) && abs.Status == AbsenceStatus.Approved));
                    if (trackedActivitiesByDay.Count == 0 && trackedAbsensesByDay.Count == 0)
                    {
                        noTrackedDays++;
                    }
                }
            }

            if (noTrackedDays > 0)
            {
                await CreateTrackingNotificationAsync(userId);
            }

            return Task.CompletedTask;
        }

        private async Task CreateNotificationNothingTracked(int userId,LocalDate startDate)
        {
            var notification = new Notification
            {
                UserId = userId,
                Caption = "Tracking Notification",
                Description = "You have nothing tracked since " + startDate,
                CreatedOn = DateTime.Now,
                IsRead = false
            };
            await _notificationRepository.CreateAsync(notification);
        }

        private async Task CreateTrackingNotificationAsync(int userId)
        {
            var notification = new Notification
            {
                UserId = userId,
                Caption = "Tracking Notification",
                Description = "You forgot to track your time!",
                CreatedOn = DateTime.Now,
                IsRead = false
            };
            await _notificationRepository.CreateAsync(notification);
        }

        public async Task<GeneralCustomerReportApiModal> CustomerGeneralReport(Guid customerUrl, bool preMonth = false, int timezoneOffset = 0)
        {
            try
            {
                var res = await (_repository as IActivityRepository).CustomerGeneralReport(customerUrl, preMonth);
                return _mapper.Map<GeneralCustomerReportApiModal>(res); ;
            }
            catch (Exception e)
            {
                return null;
            }     
        }

        public async Task<List<ActivityApiModel>> GetAllByFilterAsync(ActivityFilterApiModel model, int[] userId)
        {
            var res = await _repository.GetAllAsync(x =>
           (userId == null || userId.Length <= 0 || userId.Any(id => id == x.UserId)) &&
           (model.ProjectIds == null || model.ProjectIds.Length <= 0 || model.ProjectIds.Any(id=>id == x.ProjectId)) &&
           x.WorkedFromUtc.ConvertToLocalDateTime(DateTimeZoneProviders.Tzdb[_userRepository.GetByAsync(u => u.Id == x.UserId).Result.TimeZone]).ToDateTimeUnspecified() > model.StartDate &&
           x.WorkedToUtc.ConvertToLocalDateTime(DateTimeZoneProviders.Tzdb[_userRepository.GetByAsync(u => u.Id == x.UserId).Result.TimeZone]).ToDateTimeUnspecified() <= model.EndDate.AddDays(1).AddMilliseconds(-1));
            
            if (res.Count == 0) throw new RecordNotFoundException("No activities found");
            return _mapper.Map<List<ActivityApiModel>>(res.OrderBy(x=>x.WorkedFromUtc).ThenBy(x=>x.WorkedToUtc));
        }

        public async Task<List<ActivityApiModel>> GetAllByFilterTrackedAsync(DateTime startDate, DateTime endDate, int userId)
        {
            var res = await _repository.GetAllAsync(x => (x.UserId == userId) &&
            (startDate < x.WorkedFromUtc && x.WorkedToUtc <= endDate.AddDays(1).AddMilliseconds(-1)));
            return res == null ? null : _mapper.Map<List<ActivityApiModel>>(res.OrderBy(x => x.WorkedFromUtc).ThenBy(x => x.WorkedToUtc));
        }

        public async Task<PagedResult<ActivityApiModel>> GetPageByFilterAsync(ActivityFilterPageApiModel model, int[] userId)
        {
            var res = await _repository.GetPageAsync<ActivityApiModel>(x =>
                (userId == null || userId.Length <= 0 || userId.Any(id => id == x.UserId)) &&
                (model.ProjectIds == null || model.ProjectIds.Length <= 0 || model.ProjectIds.Any(id => id == x.ProjectId)) &&
                x.WorkedFromUtc.ConvertToLocalDateTime(DateTimeZoneProviders.Tzdb[_userRepository.GetByAsync(u => u.Id == x.UserId).Result.TimeZone]).ToDateTimeUnspecified() > model.StartDate.Date && x.WorkedToUtc.ConvertToLocalDateTime(DateTimeZoneProviders.Tzdb[_userRepository.GetByAsync(u => u.Id == x.UserId).Result.TimeZone]).ToDateTimeUnspecified() < model.EndDate.Date.AddDays(1).AddMilliseconds(-1), model.Page, model.PageSize, o => o.WorkedFromUtc);
            return res;
        }

        public async Task<List<ActivityApiModel>> GetAllByInvoiceAsync(List<InvoiceUserProject> invoiceUserProject)
        {
            var res = new List<Activity>();
            foreach (var invoiceRecord in invoiceUserProject) 
            {
                var invoice = await _invoicePipelineRepository.GetByAsync(inv => inv.Id == invoiceRecord.InvoiceId);
                var userTimeZone = _userRepository.GetByAsync(u => u.Id == invoiceRecord.UserId).Result.TimeZone;
                res.AddRange(await _repository.GetAllAsync(x => x.ProjectId == invoiceRecord.ProjectId && 
                x.UserId == invoiceRecord.UserId &&
                x.WorkedFromUtc.ConvertToLocalDateTime(DateTimeZoneProviders.Tzdb[userTimeZone]).ToDateTimeUnspecified() > invoice.StartDate &&
                x.WorkedToUtc.ConvertToLocalDateTime(DateTimeZoneProviders.Tzdb[userTimeZone]).ToDateTimeUnspecified() < invoice.EndDate.Date.AddDays(1).AddMilliseconds(-1)));
            }
            if (res.Count == 0) throw new RecordNotFoundException("No activities found");
            return _mapper.Map<List<ActivityApiModel>>(res);
        }
    }
}