using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TrackerApi.ApiModels;

namespace TrackerApi.Services.Interfaces
{
    public interface IImportDataService
    {
        Task<List<ActivityApiModel>> ReadActivities(Stream file);
    }
}
