using cTeleport.AirportMeasure.Core.Results;
using MediatR;

namespace cTeleport.AirportMeasure.Core
{
    public interface ICommand : IRequest<Result>
    {
    }

    public interface ICommand<TResult> : IRequest<Result<TResult>>
    {
    }
}