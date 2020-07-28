namespace cTeleport.AirportMeasure.Data
{
    public class AirportDto
    {
        public string Country { get; set; }

        public string City { get; set; }

        public int Hubs { get; set; }

        public string Iata { get; set; }

        public LocationDto Location { get; set; }
    }
}