using System.Linq;
using System.Threading.Tasks;
using cTeleport.AirportMeasure.Core.Results;
using MediatR;

namespace cTeleport.AirportMeasure.Core.Pipelines
{
    internal class SelectHandler<TSource, TResult> : IPipelineItemHandler<Select<TSource, TResult>, TResult>
    {
        private readonly ICustomMediator _mediator;

        public SelectHandler(ICustomMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result<TResult>> ExecuteAsync(Select<TSource, TResult> pipeline)
        {
            var sourceResult = await _mediator.ExecuteAsync(pipeline.SourcePipeline);

            if (!sourceResult.IsSuccess)
            {
                return sourceResult.Errors.ToArray();
            }
            
            var result = pipeline.Selector(sourceResult.Data);

            return result;
        }
    }
    
    internal class SelectHandler : IPipelineItemHandler<Select>
    {
        private readonly ICustomMediator _mediator;

        public SelectHandler(ICustomMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result> ExecuteAsync(Select pipeline)
        {
            var sourceResult = await _mediator.ExecuteAsync(pipeline.SourcePipeline);
            
            var result = pipeline.Selector(sourceResult);

            return result;
        }
    }
    
    internal class SelectHandler<TData> : IPipelineItemHandler<Select<TData>, TData>
    {
        private readonly ICustomMediator _mediator;

        public SelectHandler(ICustomMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result<TData>> ExecuteAsync(Select<TData> pipeline)
        {
            var sourceResult = await _mediator.ExecuteAsync(pipeline.SourcePipeline);
            
            var result = pipeline.Selector(sourceResult);

            return result;
        }
    }
}