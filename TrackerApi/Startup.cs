using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using TrackerApi.Models;
using TrackerApi.Infrastructure;
using TrackerApi.Models.Seed;
using TrackerApi.Helpers;

namespace TrackerApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            MapperConfiguration = new MapperConfiguration(cfg => { cfg.AddProfile(new ServicesMappingProfile()); });
        }

        private MapperConfiguration MapperConfiguration { get; }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddControllers().AddNewtonsoftJson();

            services.AddTransient<IDbFactory>(factory => new DbFactory(Configuration.GetConnectionString("DB")));

            services.AddDbContext<TrackerContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DB")), ServiceLifetime.Transient);

            services.AddSingleton(c => MapperConfiguration.CreateMapper());

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.WithOrigins("http://localhost:4200")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            services.AddMvc();

            services.AddTransient<IDbFactory>(factory => new DbFactory(Configuration.GetConnectionString("DB")));

            services.AddTrackerServices();

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" }); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            
            //Enable middleware to handle with custom exceptions
            app.UseMiddleware<ErrorHandlerMiddleware>();
            
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1"); });

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors("CorsPolicy");

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            SeedData.Initialize(app.ApplicationServices);
        }
    }
}