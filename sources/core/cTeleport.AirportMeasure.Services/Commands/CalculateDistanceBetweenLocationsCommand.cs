using cTeleport.AirportMeasure.Core;
using cTeleport.AirportMeasure.Data;

namespace cTeleport.AirportMeasure.Services.Commands
{
    public class CalculateDistanceBetweenLocationsCommand : ICommand<double>
    {
        public LocationModel From { get; set; }

        public LocationModel To { get; set; }

        public CalculateDistanceBetweenLocationsCommand(LocationModel from, LocationModel to)
        {
            From = from;
            To = to;
        }
    }
}