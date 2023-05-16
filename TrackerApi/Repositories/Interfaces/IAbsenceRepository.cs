using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrackerApi.Models;

namespace TrackerApi.Repositories.Interfaces
{
    public interface IAbsenceRepository : IBaseRepository<Absence>
    {
        Task<List<Absence>> GetAllIncludeUserAsync(Func<Absence, bool> filter);
    }
}