using Microsoft.Extensions.DependencyInjection;
using TrackerApi.Repositories;
using TrackerApi.Repositories.Interfaces;

namespace TrackerApi.Infrastructure
{
    public static class TrackerRepos
    {
        public static IServiceCollection AddTrackerRepos(this IServiceCollection services)
        {
            services.AddScoped<IActivityRepository, ActivityRepository>();
            services.AddScoped<IPositionRepository, PositionRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<ISkillRepository, SkillRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserRateRepository, UserRateRepository>();
            services.AddScoped<IHolidayRepository, HolidayRepository>();
            services.AddScoped<IAbsenceRepository, AbsenceRepository>();
            services.AddScoped<IUserYearRangeRepository, UserYearRangeRepository>();
            services.AddScoped<INotificationRepository, NotificationRepostory>();
            services.AddScoped<IVacancyRepository, VacancyRepository>();
            services.AddScoped<ICandidateRepository, CandidateRepository>();
            services.AddScoped<IInvoicePipelineRepository, InvoicePipelineRepository>();
            services.AddScoped<IInvoiceUserProjectRepository, InvoiceUserProjectRepository>();
            services.AddScoped<IPaymentDetailsRepository, PaymentDetailsRepository>();

            return services;
        }
    }
}