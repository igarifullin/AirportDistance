using cTeleport.AirportMeasure.Core;
using cTeleport.AirportMeasure.Data;

namespace cTeleport.AirportMeasure.Services.Commands
{
    public class CalculateDistanceBetweenAirportsCommand : ICommand<CalculateDistanceBetweenAirportsCommandResult>
    {
        public AirportModel From { get; set; }
        
        public AirportModel To { get; set; }
    }

    public class CalculateDistanceBetweenAirportsCommandResult
    {
        public string FromAirportName { get; set; }
        
        public string ToAirportName { get; set; }

        public double Distance { get; set; }
    }
}