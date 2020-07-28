using System.Threading;
using System.Threading.Tasks;
using cTeleport.AirportMeasure.Core;
using cTeleport.AirportMeasure.Core.Results;
using cTeleport.AirportMeasure.Data;
using cTeleport.AirportMeasure.Services.Integration;

namespace cTeleport.AirportMeasure.Services.Queries
{
    public class GetAirportInformationHandler : IQueryHandler<GetAirportInformation, AirportModel>
    {
        private readonly IAirportInformationProvider _airportInformationProvider;

        public GetAirportInformationHandler(IAirportInformationProvider airportInformationProvider)
        {
            _airportInformationProvider = airportInformationProvider;
        }
        
        public async Task<Result<AirportModel>> Handle(GetAirportInformation request, CancellationToken cancellationToken)
        {
            return await _airportInformationProvider.GetAirportAsync(request.IataCode);
        }

        public async Task<Result<AirportModel>> ExecuteAsync(GetAirportInformation query)
        {
            return await _airportInformationProvider.GetAirportAsync(query.IataCode);
        }
    }
}