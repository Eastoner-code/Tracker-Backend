using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Internal;
using TrackerApi.ApiModels;
using TrackerApi.Extensions.Models;
using TrackerApi.Models;
using TrackerApi.Repositories.Interfaces;
using TrackerApi.Services.Interfaces;

namespace TrackerApi.Services
{
    public class ProjectService : BaseService<ProjectApiModel, Project>, IProjectService
    {

        public ProjectService(IProjectRepository projectRepository, IMapper mapper): base(projectRepository, mapper)
        {
        }

        public override async Task<CreateUpdate<ProjectApiModel>> CreateAsync(ProjectApiModel model)
        {
            model.CustomerUrl = Guid.NewGuid();
            return await base.CreateAsync(model);
        }

        public async Task<bool> AddUsersToProjectAsync(int userId, int projectId)
        {
            var res =  await (_repository as IProjectRepository).AddUsersToProjectAsync(userId, projectId);
            return res;
        }


        public async Task<bool> RemoveUsersFromProjectAsync(int userId, int projectId)
        {
            var res = await (_repository as IProjectRepository).RemoveUsersFromProjectAsync(userId, projectId);
            return res;
        }


        public override async Task<List<ProjectApiModel>> GetAllAsync()
        {
            var res = await (_repository as IProjectRepository).GetAllAsync(x => !x.IsArchive);
            return res == null ? null : _mapper.Map<List<ProjectApiModel>>(res);
        }

        public async Task<List<ProjectApiModel>> GetProjectsByUserId(int userId)
        {
            var res = await _repository.GetAllAsync(item => item.UserProject.Any(x=> x.UserId == userId) && !item.IsArchive);
            return res == null ? null : _mapper.Map<List<ProjectApiModel>>(res);
        }

        public Task<bool> RestoreByIdAsync(int projectId)
        {
            var res = (_repository as IProjectRepository).RestoreByIdAsync(projectId);
            return res;
        }

        public async Task<PagedResult<ProjectApiModel>> GetPageByFilterAsync(ProjectFilterPageApiModel model)
        {
            var res = await _repository.GetPageAsync<ProjectApiModel>(x =>x.IsArchive == model.IsArchive , model.Page, model.PageSize, o => o.Id);
            return res;
        }
    }
}