using System;
using cTeleport.AirportMeasure.Core;
using cTeleport.AirportMeasure.Data;

namespace cTeleport.AirportMeasure.Services.Queries
{
    public class GetAirportInformation : ICacheableQuery<AirportDto> 
    {
        public string IataCode { get; }

        public GetAirportInformation(string iataCode)
        {
            IataCode = iataCode;
            Key = $"GetAirportInformation_{iataCode}";
        }

        public TimeSpan LifeTime => Constants.DefaultLongCacheLifeTime;
        public string Key { get; }
    }
}