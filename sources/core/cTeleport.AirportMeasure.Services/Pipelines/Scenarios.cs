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
        public static IPipeline<AirportDto> GetAirportInformation(string iata) =>
            new StringIsNotNullRule(iata, $"Parameter '{nameof(iata)}' is empty")
                .And(new StringMatchRegexFormatRule(iata, RegexConstants.Iata, $"Parameter '{nameof(iata)}' has invalid format. Should be IATA format"))
                .And(new GetAirportInformation(iata));

        public static IPipeline<CalculateDistanceBetweenAirportsCommandResult> CalculateDistanceBetweenAirports(string fromIata, string toIata) =>
            new StringIsNotNullRule(fromIata, "Parameter 'from' is empty")
            .And(new StringIsNotNullRule(toIata, "Parameter 'to' is empty"))
            .And(new StringMatchRegexFormatRule(fromIata, RegexConstants.Iata, "Parameter 'from' has invalid format. Should be IATA format"))
            .And(new StringMatchRegexFormatRule(toIata, RegexConstants.Iata, "Parameter 'to' has invalid format. Should be IATA format"))
            .With(
        new GetAirportInformation(fromIata),
        new GetAirportInformation(toIata),
        (x, y) => new CalculateDistanceBetweenAirportsCommand(x, y)
            );
    }
}