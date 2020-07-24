using System;
using cTeleport.AirportMeasure.Services.Integration.Impl;
using Microsoft.Extensions.DependencyInjection;

namespace cTeleport.AirportMeasure.Services.Integration
{
    public static class IntegrationRegistry
    {
        public static IServiceCollection AddIntegrationServices(this IServiceCollection services)
        {
            services.AddHttpClient<IAirportInformationProvider, AirportInformationProvider>()
                .SetHandlerLifetime(TimeSpan.FromMinutes(5));
            return services;
        }
    }
}