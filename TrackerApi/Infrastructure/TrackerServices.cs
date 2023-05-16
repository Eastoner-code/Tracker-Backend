using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using TrackerApi.Models;
using TrackerApi.Services;
using TrackerApi.Services.Interfaces;

namespace TrackerApi.Infrastructure
{
    public static class TrackerServices
    {
        public static IServiceCollection AddTrackerServices(this IServiceCollection services)
        {
            services.AddIdentity<IdentityAuthUser, IdentityAuthRole>(opt =>
            {
                opt.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<TrackerContext>();
            services.AddAuthentication("MyCookie")
            .AddCookie("MyCookie", options =>
            {
              options.ExpireTimeSpan = TimeSpan.FromDays(30);
            });


            // Import Repositories
            services.AddTrackerRepos();

            // Import All Services
            services.AddScoped<IActivityService, ActivityService>();
            services.AddScoped<IPositionService, PositionService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<ISkillService, SkillService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRateService, UserRateService>();
            services.AddScoped<IHolidayService, HolidayService>();
            services.AddScoped<IAbsenceService, AbsenceService>();
            services.AddScoped<IUserYearRangeService, UserYearRangeService>();
            services.AddScoped<ISalaryCalculationService, SalaryCalculationService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IVacancyService, VacancyService>();
            services.AddScoped<ICandidateService, CandidateService>();
            services.AddScoped<IInvoicePipelineService, InvoicePipelineService>();
            services.AddScoped<IPaymentDetailsService, PaymentDetailsService>();
            services.AddScoped<IImportDataService, ImportDataService>();

            // Import Validators
            services.AddTrackerValidators();

            return services;
        }
    }
}