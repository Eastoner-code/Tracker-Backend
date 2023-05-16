using System.Collections.Generic;
using System.Threading.Tasks;
using TrackerApi.ApiModels;

namespace TrackerApi.Services.Interfaces
{
    public interface IUserService : IBaseService<UserApiModel>
    {

        Task<IEnumerable<string>> GetUserRolesById(int userId);
        Task<IEnumerable<string>> GetAllRoles();
        Task<List<UserApiModel>> GetUserByPosition(List<int> positionIds);
        Task<UserReport> GetUserProjectReports(List<int> projectIds, int userId);
        Task<List<UserApiModel>> GetAllUsers();

        Task<List<UserApiModel>> GetNotComfirmedUsers();
        Task<UserApiModel> GetUserByUserEmail(string email);
        Task<UserApiModel> GetUserByUserId(string id);
        Task<bool> UpdateUserRoles(int userId, IEnumerable<string> roles);
        Task<List<UserApiModel>> GetUsersByProjectId(int projectId);
    }
}