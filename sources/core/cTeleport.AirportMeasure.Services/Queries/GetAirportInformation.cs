using cTeleport.AirportMeasure.Core;
using cTeleport.AirportMeasure.Data;

namespace cTeleport.AirportMeasure.Services.Queries
{
    public class GetAirportInformation : IQuery<AirportModel> 
    {
        public string IataCode { get; set; }
    }
}