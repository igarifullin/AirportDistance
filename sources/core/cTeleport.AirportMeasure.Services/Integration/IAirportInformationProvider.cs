using System.Threading.Tasks;
using cTeleport.AirportMeasure.Core.Results;
using cTeleport.AirportMeasure.Data;

namespace cTeleport.AirportMeasure.Services.Integration
{
    public interface IAirportInformationProvider
    {
        Task<Result<AirportModel>> GetAirportAsync(string iataCode);
    }
}