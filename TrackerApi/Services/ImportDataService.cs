using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrackerApi.ApiModels;
using TrackerApi.ApiModels.CSVModels;
using TrackerApi.Models;
using TrackerApi.Repositories.Interfaces;
using TrackerApi.Services.Interfaces;

namespace TrackerApi.Services
{
    public class ImportDataService : IImportDataService
    {
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IActivityService _activityService;
        private readonly IProjectService _projectService;
        public ImportDataService(IUserRepository userRepository, IActivityService activityService, IProjectRepository projectRepository, IProjectService projectService)
        {
            _activityService = activityService;
            _projectService = projectService;
            _projectRepository = projectRepository;
            _userRepository = userRepository;
        }
        public async Task<List<ActivityApiModel>> ReadActivities(Stream file)
        {
            var records = new List<ActivityCsvModel>();
            using (var reader = new StreamReader(file))
            using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                records = await csvReader.GetRecordsAsync<ActivityCsvModel>().ToListAsync();
            }
            List<ActivityApiModel> activities = await ConvertToActivityAsync(records);
            await _activityService.CreateRangeAsync(activities);
            return activities;
        }

        private async Task<List<ActivityApiModel>> ConvertToActivityAsync(List<ActivityCsvModel> records)
        {
            var activities = new List<ActivityApiModel>();
            foreach (var record in records)
            {
                var project = await _projectRepository.GetByAsync(pr => pr.Name == record.Project);
                if (project == null) project = await _projectRepository.CreateAsync(new Project { Name = record.Project, CreatedAtUtc = DateTime.UtcNow, Meta = "{}" });
                var user = await _userRepository.GetByAsync(u => u.FirstName + ' ' + u.LastName == record.UserName);
                if (user != null)
                {
                    await _projectService.AddUsersToProjectAsync(user.Id, project.Id);
                    activities.Add(new ActivityApiModel
                    {
                        Description = record.Description,
                        UserId = user.Id,
                        ProjectId = project.Id,
                        Duration = Convert.ToInt64(record.Duration) / 60,
                        WorkedFromUtc = record.WorkedFrom,
                        WorkedToUtc = record.WorkedTo,
                        CreatedAtUtc = DateTime.UtcNow
                    });
                }
            }
            return activities;
        }
    }
}
