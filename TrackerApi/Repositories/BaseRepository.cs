using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TrackerApi.Extensions;
using TrackerApi.Extensions.Models;
using TrackerApi.Models;
using TrackerApi.Models.Interfaces;
using TrackerApi.Repositories.Interfaces;

namespace TrackerApi.Repositories
{
    public abstract class BaseRepository<TModel> : IBaseRepository<TModel> where TModel : class, IBaseModel
    {
        protected readonly ILogger<BaseRepository<TModel>> _logger;
        protected readonly TrackerContext _trackerContext;
        protected readonly DbSet<TModel> _dbSet;
        protected readonly IMapper _mapper;

        protected BaseRepository(TrackerContext trackerContext, ILogger<BaseRepository<TModel>> logger, IMapper mapper)
        {
            _trackerContext = trackerContext;
            _logger = logger;
            _mapper = mapper;
            _dbSet = trackerContext.Set<TModel>();
            _mapper = mapper;
        }

        public virtual async Task<TModel> CreateAsync(TModel model)
        {
            try
            {
                model.Id = 0;
                await _dbSet.AddAsync(model);
                await _trackerContext.SaveChangesAsync();
                return model;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public virtual async Task<IEnumerable<TModel>> CreateRangeAsync(ICollection<TModel> models)
        {
            try
            {
                foreach (var model in models)
                {
                    model.Id = 0;
                }
                await _dbSet.AddRangeAsync(models);
                await _trackerContext.SaveChangesAsync();
                return models;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public virtual async Task<TModel> UpdateAsync(TModel model)
        {
            try
            {
                var local = _dbSet
                    .Local
                    .FirstOrDefault(entry => entry.Id.Equals(model.Id));

                // check if local is not null 
                if (local != null)
                {
                    // detach
                    _trackerContext.Entry(local).State = EntityState.Detached;
                }

                _dbSet.Attach(model);
                _trackerContext.Entry(model).State = EntityState.Modified;
                await _trackerContext.SaveChangesAsync();
                return model;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public virtual async Task<bool> DeleteByIdAsync(int id)
        {
            try
            {
                var found = await _dbSet.FindAsync(id);
                if (found == null)
                    return false;
                _dbSet.Remove(found);
                await _trackerContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public virtual async Task<bool> DeleteRangeAsync(IEnumerable<TModel> entities)
        {
            try
            {
                _dbSet.RemoveRange(entities);
                await _trackerContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public virtual Task<TModel> GetByAsync(Func<TModel, bool> filter)
        {
            var res = _dbSet.AsNoTracking().SingleOrDefault(filter);
            return Task.FromResult(res);
        }

        public virtual Task<List<TModel>> GetAllAsync()
        {
            var res = _dbSet.AsNoTracking().ToList();
            return Task.FromResult(res);
        }

        public virtual Task<List<TModel>> GetAllAsync(Func<TModel, bool> filter)
        {
            var res = _dbSet.AsNoTracking().Where(filter).ToList();
            return Task.FromResult(res);
        }

        public virtual Task<PagedResult<TU>> GetPageAsync<TU>(int page, int pageSize) where TU : class
        {
            var res = _dbSet.GetPaged<TModel, TU>(page, pageSize, _mapper);
            return Task.FromResult(res);
        }

        public virtual Task<PagedResult<TModel>> GetPageAsync(Func<TModel, bool> filter, int page, int pageSize)
        {
            var res = _dbSet.GetPagedWhere(filter, page, pageSize);
            return Task.FromResult(res);
        }

        public virtual Task<PagedResult<TU>> GetPageAsync<TU>(Func<TModel, bool> filter, int page, int pageSize) where TU : class
        {
            var res = _dbSet.GetPagedWhere<TModel, TU>(filter, page, pageSize, _mapper);
            return Task.FromResult(res);
        }

        public virtual Task<PagedResult<TModel>> GetPageAsync(Func<TModel, bool> filter, int page, int pageSize, Func<TModel, object> order)
        {
            var res = _dbSet.GetPagedWhereAndOrder(filter, page, pageSize, order);
            return Task.FromResult(res);
        }

        public virtual Task<PagedResult<TU>> GetPageAsync<TU>(Func<TModel, bool> filter, int page, int pageSize, Func<TModel, object> order) where TU : class
        {
            var res = _dbSet.GetPagedWhereAndOrder<TModel, TU>(filter, page, pageSize, order, _mapper);
            return Task.FromResult(res);
        }
    }
}
