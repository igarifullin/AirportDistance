namespace cTeleport.AirportMeasure.Api.ComponentTests.Models
{
    public class DistanceModel
    {
        public AirportModel From { get; set; }

        public AirportModel To { get; set; }

        public double Distance { get; set; }
    }
}