using Newtonsoft.Json;

namespace cTeleport.AirportMeasure.Data
{
    public class LocationDto
    {
        [JsonProperty(PropertyName = "lon")]
        public double Longitude { get; set; }

        [JsonProperty(PropertyName = "lat")]
        public double Latitude { get; set; }
    }
}