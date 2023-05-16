using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using TrackerApi.ApiModels;
using TrackerApi.Models;
using TrackerApi.Models.Enums;
using TrackerApi.Repositories.Interfaces;
using TrackerApi.Services.Interfaces;

namespace TrackerApi.Services
{
    public class AbsenceService : BaseService<AbsenceApiModel, Absence>, IAbsenceService
    {
        private readonly IAbsenceRepository _absenceRepository;
        private readonly IUserYearRangeService _rangeService;

        public AbsenceService(IMapper mapper, IAbsenceRepository absenceRepository, IUserYearRangeService rangeService) : base(absenceRepository, mapper)
        {
            _absenceRepository = absenceRepository;
            _rangeService = rangeService;
        }

        public async Task<IEnumerable<AbsenceApiModel>> GetMyAbsences(int yearRangeId, int userId)
        {
            var absences = await _absenceRepository.GetAllAsync(x =>
                x.UserId == userId && x.UserYearRangeId == yearRangeId);

            return absences == null ? null : _mapper.Map<IEnumerable<AbsenceApiModel>>(absences);
        }

        public async Task<IEnumerable<AbsenceApiModel>> GetApprovedAbsencesByDates(DateTime from, DateTime to, int userId)
        {
            var absences = await _absenceRepository.GetAllAsync(x =>
                x.UserId == userId && x.Status == AbsenceStatus.Approved && x.Type != AbsenceType.DayOff &&
                (x.StartDate >= @from && x.StartDate <= to || x.EndDate >= @from && x.EndDate <= to));

            return absences == null ? null : _mapper.Map<IEnumerable<AbsenceApiModel>>(absences);
        }

        public async Task<IEnumerable<AbsenceApiModel>> GetAllAbsencesByRange(DateTime from, DateTime to)
        {
            var absences = await _absenceRepository.GetAllAsync(x =>
                (x.StartDate >= @from && x.StartDate <= to || x.EndDate >= @from && x.EndDate <= to));

            return absences == null ? null : _mapper.Map<IEnumerable<AbsenceApiModel>>(absences);
        }

        public async Task<CreateUpdate<AbsenceApiModel>> RequestAsync(AbsenceApiModel model)
        {
            model.Status = AbsenceStatus.Pending;
            model.ApprovedByUserId = null;
            var mapped = _mapper.Map<Absence>(model);
            var result = await _rangeService.SubtractAbsence(mapped);

            return new CreateUpdate<AbsenceApiModel>
            {
                Success = result,
                Model = _mapper.Map<AbsenceApiModel>(mapped)
            };
        }

        public async Task<IEnumerable<AbsenceApiModel>> GetAbsencesToApprove(int userId)
        {

            var absences = await _absenceRepository.GetAllIncludeUserAsync(x => x.Status == AbsenceStatus.Pending && x.UserId != userId);

            return absences == null ? null : _mapper.Map<IEnumerable<AbsenceApiModel>>(absences);
        }

        public async Task<CreateUpdate<AbsenceApiModel>> UpdateRequestStatus(int approverUserId, int id, AbsenceStatus status)
        {
            var absence = await _absenceRepository.GetByAsync(x => x.Id == id);
            absence.Status = status;
            absence.ApprovedByUserId = approverUserId;
            var mapped = _mapper.Map<Absence>(absence);

            if (status == AbsenceStatus.Rejected)
            {
                if (!await _rangeService.ReturnDays(absence))
                {

                    return new CreateUpdate<AbsenceApiModel> { Success = false, Model = _mapper.Map<AbsenceApiModel>(mapped) };
                }
            }

            await _absenceRepository.UpdateAsync(mapped);

            return new CreateUpdate<AbsenceApiModel> { Success = true, Model = _mapper.Map<AbsenceApiModel>(mapped) };
        }

        public async Task<bool> DeleteAndReturnDaysAsync(int id)
        {
            var absence = await _absenceRepository.GetByAsync(x => x.Id == id);
            if (absence.Status != AbsenceStatus.Pending) return false;
            var result = await _rangeService.ReturnDays(absence);
            if (!result) return false;
            await _absenceRepository.DeleteByIdAsync(id);
            return true;
        }
    }
}