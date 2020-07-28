using System;
using System.Threading.Tasks;
using cTeleport.AirportMeasure.Core;
using cTeleport.AirportMeasure.Core.Results;
using cTeleport.AirportMeasure.Data;

namespace cTeleport.AirportMeasure.Services.Commands
{
    public class CalculateDistanceBetweenLocationsCommandHandler : ICommandHandler<CalculateDistanceBetweenLocationsCommand, double>
    {
        public Task<Result<double>> ExecuteAsync(CalculateDistanceBetweenLocationsCommand command)
        {
            var R = Constants.EarthRadius;
            var lat1 = command.From.Latitude;
            var lat2 = command.To.Latitude;
            var lon1 = command.From.Longitude;
            var lon2 = command.To.Longitude;

            var fi1 = lat1 * Math.PI / 180;
            var fi2 = lat2 * Math.PI / 180;
            var deltaFi = (lat2 - lat1) * Math.PI / 180;
            var deltaLambda = (lon2 - lon1) * Math.PI / 180;

            var a = Math.Sin(deltaFi / 2) * Math.Sin(deltaFi / 2) +
                    Math.Cos(fi1) * Math.Cos(fi2) *
                    Math.Sin(deltaLambda / 2) * Math.Sin(deltaLambda / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            // result distance in metres
            var d = R * c;
            var result = d * Constants.MilesInMeter;

            return Result.SuccessData(result);
        }
    }
}