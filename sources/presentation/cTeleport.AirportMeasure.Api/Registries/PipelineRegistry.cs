using cTeleport.AirportMeasure.Core;
using cTeleport.AirportMeasure.Services.Commands;
using cTeleport.AirportMeasure.Services.Queries;
using cTeleport.AirportMeasure.Services.ValidationRules;
using Microsoft.Extensions.DependencyInjection;

namespace cTeleport.AirportMeasure.Api.Registries
{
    public static class PipelineRegistry
    {
        public static IServiceCollection AddAllPipelines(this IServiceCollection services)
            =>
                services
                    .AddAllCommands(typeof(CalculateDistanceBetweenAirportsCommand).Assembly)
                    .AddAllValidations(typeof(StringIsNotNullRule).Assembly)
                    .AddAllQueries(typeof(GetAirportInformation).Assembly);
    }
}