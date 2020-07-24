using cTeleport.AirportMeasure.Core.Results;
using MediatR;

namespace cTeleport.AirportMeasure.Core
{
    public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, Result<TResult>> where TQuery : IQuery<TResult>
    {
    }
}