using cTeleport.AirportMeasure.Core;
using cTeleport.AirportMeasure.Data;

namespace cTeleport.AirportMeasure.Services.Commands
{
    public class CalculateDistanceBetweenAirportsCommand : ICommand<CalculateDistanceBetweenAirportsCommandResult>
    {
        public AirportDto From { get; set; }
        
        public AirportDto To { get; set; }

        public CalculateDistanceBetweenAirportsCommand(AirportDto from, AirportDto to)
        {
            From = from;
            To = to;
        }
    }

    public class CalculateDistanceBetweenAirportsCommandResult
    {
        public AirportDto From { get; set; }
        
        public AirportDto To { get; set; }

        public double Distance { get; set; }
    }
}