using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TrackerApi.ApiModels;
using TrackerApi.Extensions;
using TrackerApi.Models;
using TrackerApi.Repositories.Interfaces;

namespace TrackerApi.Repositories
{
    public class UserRepository : BaseRepository<IdentityAuthUser>, IUserRepository
    {
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(TrackerContext trackerContext, ILogger<UserRepository> logger, IMapper mapper) : base(trackerContext, logger, mapper)
        {
            _logger = logger;
        }

        public Task<UserReport> GetUserProjectReports(List<int> projectIds, int userId)
        {
            var today = DateTime.Now;
            var user = _trackerContext.Users.AsNoTracking().Where(x => x.Id == userId)
                .Include(x => x.Activity).FirstOrDefault();
            if (user == null)
                return null;
            user = new IdentityAuthUser
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Activity = user.Activity.Where(activity =>
                        projectIds.Any(
                            projectId =>  projectId == activity.ProjectId))
                    .ToList()
            };
            var userReport = new UserReport
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.FirstName,
                TotalHours = user.Activity.Sum(activity => activity.Duration.HasValue ? activity.Duration.Value : 0),
                CurrentWeekHours = user.Activity.Where(activity => activity.WorkedFromUtc > today.FirstDayOfWeek())
                    .Sum(activity => activity.Duration.HasValue ? activity.Duration.Value : 0),
                CurrentMonthHours = user.Activity.Where(activity => activity.WorkedFromUtc > today.FirstDayOfMonth())
                    .Sum(activity => activity.Duration.HasValue ? activity.Duration.Value : 0),
                PreviousMonthHours = user.Activity.Where(activity =>
                    activity.WorkedFromUtc > today.FirstDayOfthePreviousMonth()
                    && activity.WorkedFromUtc < today.FirstDayOfthePreviousMonth().LastDayOfMonth()
                ).Sum(activity => activity.Duration.HasValue ? activity.Duration.Value : 0)
            };

            return Task.FromResult(userReport);
        }

        public Task<List<IdentityAuthUser>> GetUsersByPositionsWithProjectAndPosition(List<int> positionIds)
        {
            var res = _trackerContext.Users.AsNoTracking()
                .Include(r => r.UserPosition)
                .Where(r => r.UserPosition.Any(x => positionIds.Any(positionId => positionId == x.Id)))
                .Include(r => r.UserProject);

            return Task.FromResult(res.ToList());
        }

        public Task<List<IdentityAuthUser>> GetAllInclideUserProjectAsync(Func<IdentityAuthUser, bool> filter)
        {
            var res = _trackerContext.Users.AsNoTracking().Include(r => r.UserProject).Where(filter);
            return Task.FromResult(res.ToList());
        }
    }
}