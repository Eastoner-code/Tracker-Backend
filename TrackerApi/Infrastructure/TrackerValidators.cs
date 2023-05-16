using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using TrackerApi.ApiModels;
using TrackerApi.Validation;

namespace TrackerApi.Infrastructure
{
    public static class TrackerValidators
    {
        public static IServiceCollection AddTrackerValidators(this IServiceCollection services)
        {
            services.AddFluentValidation(
                fv =>
                {
                    fv.ImplicitlyValidateChildProperties = true;
                    fv.ImplicitlyValidateRootCollectionElements = true;

                    fv.RegisterValidatorsFromAssemblyContaining<Startup>();
                }
                );
            services.AddScoped<IValidator<ActivityApiModel>, ActivityValidator>();
            return services;
        }
    }
}
