using System.Threading.Tasks;
using cTeleport.AirportMeasure.Core.Results;

namespace cTeleport.AirportMeasure.Core.Pipelines
{
    public interface IPipelineItemHandler<in TPipeline> 
        where TPipeline : IPipeline
    {
        Task<Result> ExecuteAsync(TPipeline pipeline);
    }

    public interface IPipelineItemHandler<in TPipeline, TResult> 
        where TPipeline : IPipeline<TResult>
    {
        Task<Result<TResult>> ExecuteAsync(TPipeline pipeline);
    }
}