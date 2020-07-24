using System;
using System.Threading;
using System.Threading.Tasks;
using cTeleport.AirportMeasure.Core.Results;

namespace cTeleport.AirportMeasure.Core
{
    internal class CacheableQueryHandler<TQuery, TResult>
        where TQuery : ICacheableQuery<TResult>
    {
        private readonly ICacheableStorage _cacheableStorage;
        private readonly Lazy<IQueryHandler<TQuery, TResult>> _queryHandler;

        public CacheableQueryHandler(
            Lazy<IQueryHandler<TQuery, TResult>> queryHandler,
            ICacheableStorage cacheableStorage)
        {
            _queryHandler = queryHandler;
            _cacheableStorage = cacheableStorage;
        }

        public async Task<Result<TResult>> ExecuteAsync(TQuery query, CancellationToken cancellationToken)
        {
            var value = _cacheableStorage.Get<TResult>(query.Key);

            if (value != null)
            {
                return Result.SuccessData(value);
            }
            
            var result = await _queryHandler.Value.Handle(query, cancellationToken);
            if (result.IsSuccess)
            {
                _cacheableStorage.Upsert(query.Key, result.Data, query.LifeTime);
            }

            return result;
        }
    }
}