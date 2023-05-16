using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrackerApi.Extensions.Models;
using TrackerApi.Models.Interfaces;

namespace TrackerApi.Repositories.Interfaces
{
    public interface IBaseRepository<TModel> where TModel : class, IBaseModel
    {
        Task<TModel> CreateAsync(TModel model);
        Task<IEnumerable<TModel>> CreateRangeAsync(ICollection<TModel> models);

        Task<TModel> UpdateAsync(TModel model);

        Task<bool> DeleteByIdAsync(int id);
        Task<bool> DeleteRangeAsync(IEnumerable<TModel> entities);

        Task<TModel> GetByAsync(Func<TModel, bool> filter);

        Task<List<TModel>> GetAllAsync();

        Task<List<TModel>> GetAllAsync(Func<TModel, bool> filter);

        Task<PagedResult<TU>> GetPageAsync<TU>(int page, int pageSize) where TU : class;

        Task<PagedResult<TModel>> GetPageAsync(Func<TModel, bool> filter, int page, int pageSize);

        Task<PagedResult<TU>> GetPageAsync<TU>(Func<TModel, bool> filter, int page, int pageSize) where TU : class;

        Task<PagedResult<TModel>> GetPageAsync(Func<TModel, bool> filter, int page, int pageSize, Func<TModel, object> order);

        Task<PagedResult<TU>> GetPageAsync<TU>(Func<TModel, bool> filter, int page, int pageSize, Func<TModel, object> order) where TU : class;

    }
}
