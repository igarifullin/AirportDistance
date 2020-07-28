using cTeleport.AirportMeasure.Services.Commands;
using Microsoft.AspNetCore.Mvc;

namespace cTeleport.AirportMeasure.Api.Models
{
    public class DistanceModel
    {
        public AirportModel From { get; set; }

        public AirportModel To { get; set; }

        public double Distance { get; set; }

        public static DistanceModel FromResult(CalculateDistanceBetweenAirportsCommandResult result)
        {
            return new DistanceModel
            {
                From = AirportModel.FromDto(result.From),
                To = AirportModel.FromDto(result.To),
                Distance = result.Distance
            };
        }

        public ObjectResult ToObjectResult()
        {
            return new OkObjectResult(this);
        }
    }
}