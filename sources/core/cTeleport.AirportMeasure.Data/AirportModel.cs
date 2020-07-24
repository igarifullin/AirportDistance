namespace cTeleport.AirportMeasure.Data
{
    public class AirportModel
    {
        public string Country { get; set; }

        public string City { get; set; }

        public int Hubs { get; set; }

        public LocationModel Location { get; set; }
    }
}