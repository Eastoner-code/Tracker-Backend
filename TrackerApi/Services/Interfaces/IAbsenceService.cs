using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrackerApi.ApiModels;
using TrackerApi.Models.Enums;

namespace TrackerApi.Services.Interfaces
{
    public interface IAbsenceService : IBaseService<AbsenceApiModel>
    {
        Task<IEnumerable<AbsenceApiModel>> GetMyAbsences(int yearRangeId, int userId);
        Task<IEnumerable<AbsenceApiModel>> GetApprovedAbsencesByDates(DateTime from, DateTime to, int userId);
        Task<CreateUpdate<AbsenceApiModel>> RequestAsync(AbsenceApiModel model);
        Task<IEnumerable<AbsenceApiModel>> GetAbsencesToApprove(int userId);
        Task<CreateUpdate<AbsenceApiModel>> UpdateRequestStatus(int approverUserId, int id, AbsenceStatus status);
        Task<bool> DeleteAndReturnDaysAsync(int id);

        Task<IEnumerable<AbsenceApiModel>> GetAllAbsencesByRange(DateTime from, DateTime to);
    }
}