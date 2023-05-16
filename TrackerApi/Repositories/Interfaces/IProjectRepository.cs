using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrackerApi.Extensions.Models;
using TrackerApi.Models;

namespace TrackerApi.Repositories.Interfaces
{
    public interface IProjectRepository: IBaseRepository<Project>
    {
        Task<bool> AddUsersToProjectAsync(int userId, int projectId);
        Task<bool> RemoveUsersFromProjectAsync(int userId, int projectId);
        Task<bool> RestoreByIdAsync(int id);
    }
}