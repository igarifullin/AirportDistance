using cTeleport.AirportMeasure.Core.Pipelines;

namespace cTeleport.AirportMeasure.Core
{
    public interface ICommand : IPipeline
    {
    }

    public interface ICommand<TResult> : IPipeline<TResult>
    {
    }
}