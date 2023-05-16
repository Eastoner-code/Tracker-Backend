using System.Collections.Generic;
using System.Threading.Tasks;
using TrackerApi.ApiModels;

namespace TrackerApi.Services.Interfaces
{
    public interface IBaseService<T>
    {
        Task<CreateUpdate<T>> CreateAsync(T model);
        Task<List<CreateUpdate<T>>> CreateRangeAsync(List<T> models);
        Task<CreateUpdate<T>> UpdateAsync(T model);
        Task<T> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync();
        Task<bool> DeleteAsync(int id);
    }
}