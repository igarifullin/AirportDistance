namespace cTeleport.AirportMeasure.Core.Pipelines
{
    public interface IPipeline
    {
        
    }

    public interface IPipeline<in TData> : IPipeline
    {
        
    }
}