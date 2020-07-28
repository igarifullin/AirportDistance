using System.Linq;
using System.Threading.Tasks;
using cTeleport.AirportMeasure.Core;
using cTeleport.AirportMeasure.Core.Results;

namespace cTeleport.AirportMeasure.Services.Commands
{
    public class CalculateDistanceBetweenAirportsCommandHandler : ICommandHandler<CalculateDistanceBetweenAirportsCommand, CalculateDistanceBetweenAirportsCommandResult>
    {
        private readonly IMediator _mediator;

        public CalculateDistanceBetweenAirportsCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        public async Task<Result<CalculateDistanceBetweenAirportsCommandResult>> ExecuteAsync(CalculateDistanceBetweenAirportsCommand command)
        {
            var distanceResult = await _mediator.ExecuteAsync(new CalculateDistanceBetweenLocationsCommand(command.From.Location, command.To.Location));
            if (!distanceResult.IsSuccess)
            {
                return distanceResult.Errors.ToArray();
            }

            var result = new CalculateDistanceBetweenAirportsCommandResult
            {
                Distance = distanceResult.Data,
                From = command.From,
                To = command.To
            };
            return result;
        }
    }
}