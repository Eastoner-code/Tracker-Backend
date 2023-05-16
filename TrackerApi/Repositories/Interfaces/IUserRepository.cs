using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrackerApi.ApiModels;
using TrackerApi.Models;

namespace TrackerApi.Repositories.Interfaces
{
    public interface IUserRepository: IBaseRepository<IdentityAuthUser>
    {
        Task<List<IdentityAuthUser>> GetUsersByPositionsWithProjectAndPosition(List<int> positionIds);

        Task<UserReport> GetUserProjectReports(List<int> projectIds, int userId);

        Task<List<IdentityAuthUser>> GetAllInclideUserProjectAsync(Func<IdentityAuthUser, bool> filter);
    }
}