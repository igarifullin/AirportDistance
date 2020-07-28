using cTeleport.AirportMeasure.Api.Registries;
using cTeleport.AirportMeasure.Core;
using cTeleport.AirportMeasure.Data;
using cTeleport.AirportMeasure.Data.Configuration;
using cTeleport.AirportMeasure.DataAccess;
using cTeleport.AirportMeasure.Services.Integration;
using cTeleport.AirportMeasure.Services.Storages;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace cTeleport.AirportMeasure.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ConnectionStringsConfig>(Configuration.GetSection("connectionStrings"));
            services.Configure<AppSettings>(Configuration.GetSection("appSettings"));
            var appSettings = Configuration.GetSection("appSettings").Get<AppSettings>();
            Constants.DefaultLongCacheLifeTime = appSettings.LongCacheLifetime;
            
            services.AddControllers();
            services.AddRequests()
                .AddAllPipelines()
                .AddIntegrationServices(Configuration);

            if (Environment.IsDevelopment())
            {
                services.AddInMemoryCacheableStorage();
            }
            else
            {
                services.AddRedisCache(Configuration);
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}