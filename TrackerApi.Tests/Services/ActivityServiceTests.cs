using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Threading.Tasks;
using TrackerApi.Extensions;
using TrackerApi.Infrastructure;
using TrackerApi.Models;
using TrackerApi.Models.Enums;
using TrackerApi.Repositories;
using TrackerApi.Repositories.Interfaces;
using TrackerApi.Services;
using TrackerApi.Services.Interfaces;

namespace TrackerApi.Tests.Services
{
    class ActivityServiceTests
    {
        private static IActivityService _activityService;
        private static INotificationRepository _notificationRepository;
        private static IAbsenceRepository _absenceRepository;
        private static IActivityRepository _activityRepository;
        private static IUserRepository _userRepository;
        private static ILogger<BaseRepository<Notification>> _loggerNotificationRepository;
        private static ILogger<BaseRepository<Absence>> _loggerAbsenceRepository;
        private static ILogger<ActivityRepository> _loggerActivityRepository;
        private static ILogger<UserRepository> _loggerUserRepository;
        public ActivityServiceTests()
        {
            var builder = new DbContextOptionsBuilder<TrackerContext>().UseInMemoryDatabase(databaseName: "Test");
            var context = new TrackerContext(builder.Options);
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ServicesMappingProfile());
            });
            IMapper _mapper = mapperConfig.CreateMapper();

            _loggerNotificationRepository = Substitute.For<ILogger<BaseRepository<Notification>>>();
            _notificationRepository = new NotificationRepostory(context, _loggerNotificationRepository, _mapper);

            _loggerAbsenceRepository = Substitute.For<ILogger<BaseRepository<Absence>>>();
            _absenceRepository = new AbsenceRepository(context, _loggerAbsenceRepository, _mapper);

            _loggerActivityRepository = Substitute.For<ILogger<ActivityRepository>>();
            _activityRepository = new ActivityRepository(context, _loggerActivityRepository, _mapper);

            _loggerUserRepository = Substitute.For<ILogger<UserRepository>>();
            _userRepository = new UserRepository(context, _loggerUserRepository, _mapper);
            _activityService = new ActivityService(_activityRepository, _mapper, _notificationRepository, _absenceRepository, _userRepository);

            SeedDb(context);
        }

        private void SeedDb(TrackerContext context)
        {
            var absense = new Absence()
            {
                StartDate = new DateTime(2021, 10, 31, 22, 0, 0),
                EndDate = new DateTime(2021, 11, 5, 22, 0, 0),
                Status = AbsenceStatus.Approved,
                UserId = 1
            };
            context.Absences.Add(absense);
            var user = new IdentityAuthUser()
            {
                Id = 1,
                UserName = "test",
                TimeZone = "Europe/Kiev"
            };
            context.Users.Add(user);
            context.SaveChanges();
        }

        [Test]
        public static async Task TrackingNotificationTestAsync()
        {
            await _activityService.CheckUserTrackings(1);
            var result = await _notificationRepository.GetByAsync(notif => notif.UserId == 1);

            var endDate = DateTime.UtcNow;
            var startDate = endDate.StartDateForActivityNotification();
            if (startDate.Date != endDate.Date)
            {
                if(result != null) Assert.AreEqual(result.Description, "You forgot to track your time!");
            }
        }
    }
}
