using cTeleport.AirportMeasure.Core.Extensions;
using cTeleport.AirportMeasure.Core.Pipelines;
using cTeleport.AirportMeasure.Data;
using cTeleport.AirportMeasure.Services.Commands;
using cTeleport.AirportMeasure.Services.Queries;
using cTeleport.AirportMeasure.Services.ValidationRules;

namespace cTeleport.AirportMeasure.Services.Pipelines
{
    public class Scenarios
    {
        public static IPipeline<CalculateDistanceBetweenAirportsCommandResult> CalculateDistanceBetweenAirports(string fromIata, string toIata) =>
            new StringIsNotNullRule(fromIata)
            .And(new StringIsNotNullRule(toIata))
            .And(new StringMatchRegexFormatRule(fromIata, RegexConstants.Iata))
            .And(new StringMatchRegexFormatRule(toIata, RegexConstants.Iata))
            .With(
        new GetAirportInformation(fromIata),
        new GetAirportInformation(toIata),
        (x, y) => new CalculateDistanceBetweenAirportsCommand(x, y)
            );
    }
}