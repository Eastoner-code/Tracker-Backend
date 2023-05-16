using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TrackerApi.Models;
using TrackerApi.Repositories.Interfaces;

namespace TrackerApi.Repositories
{
    public class ProjectRepository : BaseRepository<Project>, IProjectRepository
    {
        
        public ProjectRepository(TrackerContext trackerContext, ILogger<ProjectRepository> logger, IMapper mapper) : base(trackerContext, logger, mapper)
        {
        }

        public override Task<List<Project>> GetAllAsync(Func<Project, bool> filter)
        {
            var res = _trackerContext.Project.AsNoTracking().Include(X=>X.UserProject).Where(filter).ToList();
            return Task.FromResult(res);
        }


        public Task<bool> AddUsersToProjectAsync(int userId, int projectId)
        {
            var userProject = _trackerContext.UserProject.FirstOrDefault(x => x.UserId == userId && x.ProjectId == projectId);
            if (userProject == null)
            {
                userProject = _trackerContext.UserProject.Add(new UserProject { UserId = userId, ProjectId = projectId }).Entity;
                _trackerContext.SaveChanges();
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }


        public Task<bool> RemoveUsersFromProjectAsync(int userId, int projectId)
        {
            var userProject = _trackerContext.UserProject.FirstOrDefault(x => x.UserId == userId && x.ProjectId == projectId);
            
            if (userProject!= null)
            {
               _trackerContext.UserProject.Remove(userProject);
               _trackerContext.SaveChanges();
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }


        public override async Task<bool> DeleteByIdAsync(int id)
        {
            try
            {
                var found = await _dbSet.FindAsync(id);
                if (found == null)
                    return false;
                found.IsArchive = true;
                _dbSet.Update(found);
                await _trackerContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<bool> RestoreByIdAsync(int id)
        {
            try
            {
                var found = await _dbSet.FindAsync(id);
                if (found == null)
                    return false;
                found.IsArchive = false;
                _dbSet.Update(found);
                await _trackerContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}