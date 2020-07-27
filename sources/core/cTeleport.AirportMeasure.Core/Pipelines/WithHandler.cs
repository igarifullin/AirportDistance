using System.Linq;
using System.Threading.Tasks;
using cTeleport.AirportMeasure.Core.Results;

namespace cTeleport.AirportMeasure.Core.Pipelines
{
    internal class WithHandler<TData, TResult> : IPipelineItemHandler<With<TData, TResult>, TData>
    {
        private readonly ICustomMediator _mediator;

        public WithHandler(ICustomMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result<TData>> ExecuteAsync(With<TData, TResult> pipeline)
        {
            var stageResult = await _mediator.ExecuteAsync(pipeline.From);
                
            if (!stageResult.IsSuccess)
            {
                return stageResult.Errors.ToArray();
            }

            if (stageResult is ValidationResult boolResult
                && boolResult.Data == false)
            {
                return stageResult.Errors.ToArray();
            }

            var secondPipeline = pipeline.ToFunc(stageResult.Data);
            return await _mediator.ExecuteAsync(secondPipeline);
        }
    }

    internal class WithHandler<TData, T1, T2> : IPipelineItemHandler<With<TData, T1, T2>, TData>
    {
        private readonly ICustomMediator _mediator;

        public WithHandler(ICustomMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result<TData>> ExecuteAsync(With<TData, T1, T2> pipeline)
        {
            var stageResult1 = await _mediator.ExecuteAsync(pipeline.From1);
            if (!stageResult1.IsSuccess)
            {
                return stageResult1.Errors.ToArray();
            }

            if (stageResult1 is ValidationResult boolResult1
                && boolResult1.Data == false)
            {
                return stageResult1.Errors.ToArray();
            }
            
            var stageResult2 = await _mediator.ExecuteAsync(pipeline.From2);
            if (!stageResult2.IsSuccess)
            {
                return stageResult2.Errors.ToArray();
            }

            if (stageResult2 is ValidationResult boolResult2
                && boolResult2.Data == false)
            {
                return stageResult2.Errors.ToArray();
            }

            var finalPipeline = pipeline.ToFunc(stageResult1.Data, stageResult2.Data);
            return await _mediator.ExecuteAsync(finalPipeline);
        }
    }
}