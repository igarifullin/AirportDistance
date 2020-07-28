using cTeleport.AirportMeasure.Data;

namespace cTeleport.AirportMeasure.Api.Models
{
    public class AirportModel
    {
        public string Country { get; set; }

        public string City { get; set; }

        public string Iata { get; set; }

        public static AirportModel FromDto(AirportDto airportDto)
        {
            return new AirportModel
            {
                Country = airportDto.Country,
                City = airportDto.City,
                Iata = airportDto.Iata
            };
        }
    }
}