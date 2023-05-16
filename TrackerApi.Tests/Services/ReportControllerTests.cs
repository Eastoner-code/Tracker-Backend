using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TrackerApi.ApiModels;
using TrackerApi.Controllers;
using TrackerApi.Infrastructure;
using TrackerApi.Models;
using TrackerApi.Repositories;
using TrackerApi.Repositories.Interfaces;
using TrackerApi.Services;
using TrackerApi.Services.Interfaces;

namespace TrackerApi.Tests.Services
{
    class ReportControllerTests
    {
        private readonly UserManager<IdentityAuthUser> _userManager;
        private readonly IActivityService _activityService;
        
        private readonly INotificationRepository _notificationRepository;
        private readonly IAbsenceRepository _absenceRepository;
        private readonly IUserRepository _userRepository;
        private readonly IActivityRepository _activityRepository;

        private readonly ILogger<BaseRepository<Notification>> _loggerNotificationRepository;
        private readonly ILogger<BaseRepository<Absence>> _loggerAbsenceRepository;
        private readonly ILogger<ActivityRepository> _loggerActivityRepository;
        private readonly ILogger<UserRepository> _loggerUserRepository;

        private readonly ReportsController _reportsController;
        public ReportControllerTests()
        {
            var builder = new DbContextOptionsBuilder<TrackerContext>().UseInMemoryDatabase(databaseName: "Test");
            var context = new TrackerContext(builder.Options);
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ServicesMappingProfile());
            });
            IMapper _mapper = mapperConfig.CreateMapper();

            _userManager = CreateUserManager();

            _loggerNotificationRepository = Substitute.For<ILogger<BaseRepository<Notification>>>();
            _notificationRepository = new NotificationRepostory(context, _loggerNotificationRepository, _mapper);

            _loggerAbsenceRepository = Substitute.For<ILogger<BaseRepository<Absence>>>();
            _absenceRepository = new AbsenceRepository(context, _loggerAbsenceRepository, _mapper);

            _loggerActivityRepository = Substitute.For<ILogger<ActivityRepository>>();
            _activityRepository = new ActivityRepository(context, _loggerActivityRepository, _mapper);

            _loggerUserRepository = Substitute.For<ILogger<UserRepository>>();
            _userRepository = new UserRepository(context, _loggerUserRepository, _mapper);

            _activityService = new ActivityService(_activityRepository, _mapper, _notificationRepository, _absenceRepository, _userRepository);
            _reportsController = new ReportsController(_activityService, _userManager);

            SeedDb(context);
        }

        private UserManager<IdentityAuthUser> CreateUserManager()
        {
            var userStoreMock = Substitute.For<IUserStore<IdentityAuthUser>>();
            var optionsMock = Substitute.For<IOptions<IdentityOptions>>();
            var passwordHasherMock = Substitute.For<IPasswordHasher<IdentityAuthUser>>();
            var userValidatorMock = Substitute.For<IEnumerable<IUserValidator<IdentityAuthUser>>>();
            var passwordValidatorMock = Substitute.For<IEnumerable<IPasswordValidator<IdentityAuthUser>>>();
            var lookUpNormalizerMock = Substitute.For<ILookupNormalizer>();
            var identityErrorDescriberMock = Substitute.For<IdentityErrorDescriber>();
            var serviceProviderMock = Substitute.For<IServiceProvider>();
            var loggerMock = Substitute.For<ILogger<UserManager<IdentityAuthUser>>>();

            var userManagerMock = Substitute.For<UserManager<IdentityAuthUser>>(
                userStoreMock,
                optionsMock,
                passwordHasherMock,
                userValidatorMock,
                passwordValidatorMock,
                lookUpNormalizerMock,
                identityErrorDescriberMock,
                serviceProviderMock,
                loggerMock);
            return userManagerMock;
        }

        private void SeedDb(TrackerContext context)
        {
            var user = new IdentityAuthUser()
            {
                Id = 1,
                FirstName = "test",
                TimeZone = "Europe/Kiev",
                Email = "test"
            };
            context.Users.Add(user);
            context.SaveChanges();
            _userManager.FindByIdAsync(Convert.ToString(1)).Returns(Task.FromResult(user));
            
            var claim = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                                        new Claim(ClaimTypes.NameIdentifier, "1"),
                                        new Claim(ClaimTypes.Email, "test") }));
            
            _reportsController.ControllerContext.HttpContext = new DefaultHttpContext { User = claim };
            
            var project = new Project()
            {
                Id = 1,
                Name = "testProject"
            };
            context.Project.Add(project);
            for(int i = 10; i < 13; i++)
            {
                var activity = new Activity()
                {
                    Id = i,
                    Description = "testActivity",
                    ProjectId = project.Id,
                    UserId = user.Id,
                    WorkedFromUtc = new DateTime(2021, 10, i, 10, 0, 0),
                    WorkedToUtc = new DateTime(2021, 10, i, 15, 0, 0),
                    Duration = 300
                };
                context.Activity.Add(activity);
                var secondActivity = new Activity()
                {
                    Id = i*2,
                    Description = "testActivity",
                    ProjectId = project.Id,
                    UserId = user.Id,
                    WorkedFromUtc = new DateTime(2021, 10, i, 16, 0, 0),
                    WorkedToUtc = new DateTime(2021, 10, i, 18, 0, 0),
                    Duration = 120
                };
                context.Activity.Add(secondActivity);
            }
            context.SaveChanges();
        }
        [Test]
        public void ReportTotalTimeCalculationTest()
        {
            var totalTime = new List<TimeSpan>()
            {
                new TimeSpan(7,0,0),
                new TimeSpan(7,0,0),
                new TimeSpan(7,0,0),
            };
            var filter = new ReportFilterApiModel()
            {
                StartDate = new DateTime(2021, 10, 10, 0, 0, 0),
                EndDate = new DateTime(2021, 10, 13, 0, 0, 0),
                UserIds = new int[] { 1 }
            };
            var actionResult = _reportsController.GetByFilterReport(filter).Result;
            OkObjectResult okResult = actionResult as OkObjectResult;
            ReportApiModel report = okResult.Value as ReportApiModel;
            CollectionAssert.AreEqual(totalTime, report.TotalTime);
        }
    }
}
