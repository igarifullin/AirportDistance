using System;
using cTeleport.AirportMeasure.Data.Configuration;
using cTeleport.AirportMeasure.Services.Integration.Impl;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace cTeleport.AirportMeasure.Services.Integration
{
    public static class IntegrationRegistry
    {
        public static IServiceCollection AddIntegrationServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            var cTeleportUri = new Uri(configuration.GetConnectionString(nameof(ConnectionStringsConfig.CTeleportUrl)), UriKind.Absolute);
            services.AddHttpClient<IAirportInformationProvider, AirportInformationProvider>(
                    c => c.BaseAddress = cTeleportUri)
                .SetHandlerLifetime(TimeSpan.FromMinutes(5));
            return services;
        }
    }
}