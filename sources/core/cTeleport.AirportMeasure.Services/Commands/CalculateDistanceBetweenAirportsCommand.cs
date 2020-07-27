using cTeleport.AirportMeasure.Core;
using cTeleport.AirportMeasure.Core.Results;
using cTeleport.AirportMeasure.Data;

namespace cTeleport.AirportMeasure.Services.Commands
{
    public class CalculateDistanceBetweenAirportsCommand : ICommand<CalculateDistanceBetweenAirportsCommandResult>
    {
        public AirportModel From { get; set; }
        
        public AirportModel To { get; set; }

        public CalculateDistanceBetweenAirportsCommand(AirportModel from, AirportModel to)
        {
            From = from;
            To = to;
        }
    }

    public class CalculateDistanceBetweenAirportsCommandResult
    {
        public AirportModel From { get; set; }
        
        public AirportModel To { get; set; }

        public double Distance { get; set; }
    }
}