using System.Linq;
using System.Threading.Tasks;
using cTeleport.AirportMeasure.Core.Results;
using MediatR;

namespace cTeleport.AirportMeasure.Core.Pipelines
{
    internal class AndHandler : IPipelineItemHandler<And>
    {
        private readonly ICustomMediator _mediator;

        public AndHandler(ICustomMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result> ExecuteAsync(And pipeline)
        {
            foreach (var stage in pipeline.Pipelines)
            {
                var stageResult = await _mediator.ExecuteAsync(stage);

                if (!stageResult.IsSuccess)
                {
                    return stageResult;
                }

                if (stageResult is ValidationResult boolResult
                    && boolResult.Data == false)
                {
                    return stageResult.Errors.ToArray();
                }
            }

            return Result.Success;
        }
    }

    internal class AndHandler<TData> : IPipelineItemHandler<And<TData>, TData>
    {
        private readonly ICustomMediator _mediator;

        public AndHandler(ICustomMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result<TData>> ExecuteAsync(And<TData> pipeline)
        {
            foreach (var stage in pipeline.Pipelines)
            {
                var stageResult = await _mediator.ExecuteAsync(stage);
                
                if (!stageResult.IsSuccess)
                {
                    return stageResult.Errors.ToArray();
                }

                if (stageResult is ValidationResult boolResult
                    && boolResult.Data == false)
                {
                    return stageResult.Errors.ToArray();
                }
            }

            return await _mediator.ExecuteAsync(pipeline.Last);
        }
    }
}