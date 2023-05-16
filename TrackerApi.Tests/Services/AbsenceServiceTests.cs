using NUnit.Framework;
using AutoMapper;
using NSubstitute;
using TrackerApi.Repositories.Interfaces;
using TrackerApi.Services;

namespace TrackerApi.Tests.Services
{
    [TestFixture]
    public class AbsenceServiceTests
    {
        private IMapper _mapper;
        private IAbsenceRepository _absenceRepository;

        [SetUp]
        public void Setup()
        {
            _mapper = Substitute.For<IMapper>();
            _absenceRepository = Substitute.For<IAbsenceRepository>();
        }

        [Test]
        public void GetMonthDiff_StateUnderTest_ExpectedBehavior()
        {
 
        }
    }
}
