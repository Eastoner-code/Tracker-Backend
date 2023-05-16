using System.Collections.Generic;
using System.Threading.Tasks;
using TrackerApi.ApiModels;
using TrackerApi.Extensions.Models;

namespace TrackerApi.Services.Interfaces
{
    public interface IProjectService : IBaseService<ProjectApiModel>
    {
        public Task<List<ProjectApiModel>> GetProjectsByUserId(int id);
        Task<PagedResult<ProjectApiModel>> GetPageByFilterAsync(ProjectFilterPageApiModel model);
        public Task<bool> AddUsersToProjectAsync(int userId, int projectId);
        public Task<bool> RemoveUsersFromProjectAsync(int userId, int projectId);
        Task<bool> RestoreByIdAsync(int id);
    }
}