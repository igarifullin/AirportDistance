using cTeleport.AirportMeasure.Core.Results;
using MediatR;

namespace cTeleport.AirportMeasure.Core
{
    public interface IQuery<T> : IRequest<Result<T>>
    {
    }
}