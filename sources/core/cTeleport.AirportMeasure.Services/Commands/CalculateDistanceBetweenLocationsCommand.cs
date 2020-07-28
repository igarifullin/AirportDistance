using cTeleport.AirportMeasure.Core;
using cTeleport.AirportMeasure.Data;

namespace cTeleport.AirportMeasure.Services.Commands
{
    public class CalculateDistanceBetweenLocationsCommand : ICommand<double>
    {
        public LocationDto From { get; set; }

        public LocationDto To { get; set; }

        public CalculateDistanceBetweenLocationsCommand(LocationDto from, LocationDto to)
        {
            From = from;
            To = to;
        }
    }
}