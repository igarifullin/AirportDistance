using System.Threading.Tasks;
using cTeleport.AirportMeasure.Core.Results;

namespace cTeleport.AirportMeasure.Core
{
    public interface IQueryHandler<in TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        Task<Result<TResult>> ExecuteAsync(TQuery query);
    }
}