using System.Threading.Tasks;
using cTeleport.AirportMeasure.Core;
using cTeleport.AirportMeasure.Core.Results;
using cTeleport.AirportMeasure.Data;
using cTeleport.AirportMeasure.Services.Integration;

namespace cTeleport.AirportMeasure.Services.Queries
{
    public class GetAirportInformationHandler : IQueryHandler<GetAirportInformation, AirportDto>
    {
        private readonly IAirportInformationProvider _airportInformationProvider;

        public GetAirportInformationHandler(IAirportInformationProvider airportInformationProvider)
        {
            _airportInformationProvider = airportInformationProvider;
        }

        public async Task<Result<AirportDto>> ExecuteAsync(GetAirportInformation query)
        {
            return await _airportInformationProvider.GetAirportAsync(query.IataCode);
        }
    }
}