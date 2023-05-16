using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TrackerApi.ApiModels;
using TrackerApi.Extensions.ApiModels;
using TrackerApi.Models;
using TrackerApi.Models.Enums;
using TrackerApi.Repositories.Interfaces;
using TrackerApi.Services.Interfaces;

namespace TrackerApi.Services
{
    public class UserYearRangeService : BaseService<UserYearRangeApiModel, UserYearRange>, IUserYearRangeService
    {
        private readonly IUserRepository _userRepository;
        private new readonly IUserYearRangeRepository _repository;
        public UserYearRangeService(IUserYearRangeRepository repository, IMapper mapper, IUserRepository userRepository) : base(repository, mapper)
        {
            _repository = repository;
            _userRepository = userRepository;
        }

        public async Task CheckRangesAndAbsences()
        {
            var utcNow = DateTime.UtcNow;
            var users = await _userRepository.GetAllAsync();
            var ranges = await _repository.GetAllAsync();
            foreach (var user in users)
            {
                await CreateMissingRanges(user, ranges, utcNow);
            }

            ranges = await _repository.GetAllAsync();
            await CalculateNewVacations(users, ranges, utcNow);
        }

        public async Task<IEnumerable<UserYearRangeApiModel>> GetRangesByUserId(int userId)
        {
            var items = (await _repository.GetAllAsync(x => x.UserId == userId)).OrderBy(x => x.From);
            return _mapper.Map<IEnumerable<UserYearRangeApiModel>>(items);
        }

        public async Task<bool> SubtractAbsence(Absence entity)
        {
            entity.EndDate = entity.EndDate.AddDays(1).AddMilliseconds(-1);
            entity.EndDateLocal = entity.EndDateLocal.AddDays(1).AddMilliseconds(-1);
            var absence = _mapper.Map<AbsenceApiModel>(entity);
            var range = await _repository.GetCurrentRange(absence.UserId);
            double totalDays = absence.GetAbsenceTotalDays();

            if (range == null)
            {
                throw new Exception("Range not found.");
            }

            switch (absence.Type)
            {
                case AbsenceType.Vacation when range.VacationCount >= totalDays:
                    range.VacationCount -= totalDays;
                    break;
                case AbsenceType.Vacation:
                    return false;
                case AbsenceType.SickLeave when range.SickLeaveCount >= totalDays:
                    range.SickLeaveCount -= totalDays;
                    break;
                case AbsenceType.SickLeave:
                    return false;
            }

            range.Absences.Add(entity);

            await _repository.UpdateAsync(range);

            return true;
        }


        public async Task<bool> ReturnDays(Absence entity)
        {
            var absence = _mapper.Map<AbsenceApiModel>(entity);
            var totalDays = absence.GetAbsenceTotalDays();
            var range = await _repository.GetCurrentRange(absence.UserId);
            switch (absence.Type)
            {
                case AbsenceType.Vacation:
                    range.VacationCount += totalDays;
                    break;
                case AbsenceType.SickLeave:
                    range.SickLeaveCount += totalDays;
                    break;
            }

            await _repository.UpdateAsync(range);
            return true;
        }

        public async Task<UserYearRangeFullApiModel> GetDaysSummaryByRangeId(int rangeId)
        {
            var result = new UserYearRangeFullApiModel();
            var range = await _repository.GetRangeWithAbsences(rangeId);

            result.AvailableSickLeaves = range.SickLeaveCount;
            result.AvailableVacations = range.VacationCount;

            var absences = _mapper.Map<IEnumerable<AbsenceApiModel>>(range.Absences);

            foreach (var absence in absences)
            {
                switch (absence.Type)
                {
                    case AbsenceType.SickLeave:
                        result.UsedSickLeaves += absence.GetAbsenceTotalDays();
                        break;
                    case AbsenceType.Vacation:
                        result.UsedVacations += absence.GetAbsenceTotalDays();
                        break;
                    case AbsenceType.DayOff:
                        result.UsedDayOffs += absence.GetAbsenceTotalDays();
                        break;
                }
            }

            return result;
        }

        private async Task CalculateNewVacations(ICollection<IdentityAuthUser> users, ICollection<UserYearRange> ranges, DateTime utcNow)
        {
            foreach (var user in users)
            {
                var userRanges = ranges.Where(x => x.UserId == user.Id).ToArray();
                if (!userRanges.Any()) continue;
                var currentRange = userRanges.FirstOrDefault(x => x.From <= utcNow && utcNow <= x.To);
                if (currentRange == null) continue;

                var monthDiff = currentRange.From.GetMonthDiff(utcNow);

                if (currentRange.CalculatedMonth == 0)
                {
                    var previousRange = userRanges.FirstOrDefault(x => x.To == currentRange.From);
                    if (previousRange != null && !previousRange.IsVacationsTransferredToNextRange)
                    {
                        currentRange.VacationCount = Math.Round((previousRange.VacationCount / 2), 0);
                        previousRange.IsVacationsTransferredToNextRange = true;
                        await _repository.UpdateAsync(currentRange);
                        await _repository.UpdateAsync(previousRange);
                    }
                }

                if (currentRange.CalculatedMonth >= monthDiff) continue;

                var monthToAdd = monthDiff - currentRange.CalculatedMonth;
                var vacationToAdd = monthToAdd * 1.5;

                currentRange.CalculatedMonth = monthDiff;
                currentRange.VacationCount += vacationToAdd;

                await _repository.UpdateAsync(currentRange);
            }
        }

        private async Task CreateMissingRanges(IdentityAuthUser user, ICollection<UserYearRange> ranges, DateTime utcNow)
        {
            if (user.StartDateUtc != null)
            {
                var userRanges = ranges.Where(x => x.UserId == user.Id).ToArray();
                if (userRanges.Any())
                {
                    var currentRange = userRanges.FirstOrDefault(x => x.From <= utcNow && utcNow <= x.To);
                    if (currentRange == null)
                    {
                        var generatedRanges = GenerateUserRanges(user.StartDateUtc.Value, utcNow, user.Id);

                        foreach (var generatedRange in generatedRanges)
                        {
                            var savedRange = userRanges.FirstOrDefault(x =>
                                x.From == generatedRange.From && x.To == generatedRange.To);
                            if (savedRange == null)
                            {
                                await _repository.CreateAsync(generatedRange);
                            }
                        }
                    }
                }
                else
                {
                    var newRanges = GenerateUserRanges(user.StartDateUtc.Value, utcNow, user.Id);
                    await _repository.CreateRangeAsync(newRanges.ToArray());
                }

            }
        }

        private IEnumerable<UserYearRange> GenerateUserRanges(DateTime @from, DateTime to, int userId)
        {
            var rangeList = new List<UserYearRange>();
            var monthDiff = @from.GetMonthDiff(to);

            if (monthDiff < 12)
            {
                var range = GetOneYearRange(@from, userId);
                rangeList.Add(range);
            }
            else
            {
                var rangeFrom = from;
                for (int i = 0; i < monthDiff / 12 + 1; i++)
                {
                    var range = GetOneYearRange(rangeFrom, userId);
                    rangeFrom = range.To;
                    rangeList.Add(range);
                }
            }

            return rangeList;
        }

        private static UserYearRange GetOneYearRange(DateTime @from, int userId)
        {
            var range = new UserYearRange();
            range.From = @from;
            range.To = @from.AddMonths(12);
            range.UserId = userId;
            range.SickLeaveCount = 5;
            range.VacationCount = 0;
            return range;
        }

    }

}