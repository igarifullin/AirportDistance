using System.Threading;
using System.Threading.Tasks;
using cTeleport.AirportMeasure.Services.Commands;
using MediatR;

namespace cTeleport.AirportMeasure.Services.Pipelines
{
    public class CalculateDistanceBetweenAirportsPipeline : IPipelineBehavior<CalculateDistanceBetweenAirportsCommand, CalculateDistanceBetweenAirportsCommandResult>
    {
        public CalculateDistanceBetweenAirportsPipeline(IMediator mediator)
        {

        }
        
        public Task<CalculateDistanceBetweenAirportsCommandResult> Handle(CalculateDistanceBetweenAirportsCommand request, CancellationToken cancellationToken,
            RequestHandlerDelegate<CalculateDistanceBetweenAirportsCommandResult> next)
        {
            throw new System.NotImplementedException();
        }
    }
}