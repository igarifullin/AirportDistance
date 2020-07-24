using System;
using System.Threading;
using System.Threading.Tasks;
using cTeleport.AirportMeasure.Core;
using cTeleport.AirportMeasure.Core.Results;
using MediatR;

namespace cTeleport.AirportMeasure.Services.Commands
{
    public class CalculateDistanceBetweenAirportsCommandHandler : ICommandHandler<CalculateDistanceBetweenAirportsCommand, CalculateDistanceBetweenAirportsCommandResult>
    {
        private readonly IMediator _mediator;

        public CalculateDistanceBetweenAirportsCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        public Task<Result<CalculateDistanceBetweenAirportsCommandResult>> Handle(CalculateDistanceBetweenAirportsCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}