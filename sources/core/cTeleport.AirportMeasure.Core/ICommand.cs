using cTeleport.AirportMeasure.Core.Pipelines;
using cTeleport.AirportMeasure.Core.Results;
using MediatR;

namespace cTeleport.AirportMeasure.Core
{
    public interface ICommand : IRequest<Result>, IPipeline
    {
    }

    public interface ICommand<TResult> : IRequest<Result<TResult>>, IPipeline<TResult>
    {
    }
}