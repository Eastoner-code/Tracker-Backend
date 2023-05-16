using AutoMapper;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using TrackerApi.Repositories.Interfaces;
using TrackerApi.Services;
using TrackerApi.Services.Interfaces;

namespace TrackerApi.Tests.Services
{
    [TestFixture]
    public class SalaryCalculationServiceTests
    {
        private static IActivityService _activityService;
        private static IMapper _mapper;

        [SetUp]
        public static void SetUp()
        {
            _activityService = Substitute.For<IActivityService>();
            _mapper = Substitute.For<IMapper>();
        }

        [Test]
        public static void GetSalaryByUserIdMonth_Scenario_ExpectationAsync()
        {
            //var target = new SalaryCalculationService(
            //    _activityService,
            //    _mapper);

            //var r = target.GetSalaryByUserIdMonth(1, 11, 2020).Result;

            //Assert.IsTrue(false);
        }
    }
}
