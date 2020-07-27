using System;
using cTeleport.AirportMeasure.Core.Results;

namespace cTeleport.AirportMeasure.Core.Pipelines
{
    internal interface ISelect<in TData> : IPipeline<TData>
    {
    }
    
    internal class Select<TSource, TResult> : ISelect<TResult>
    {
        public readonly IPipeline<TSource> SourcePipeline; 
        public readonly Func<TSource, Result<TResult>> Selector;
        
        public Select(IPipeline<TSource> sourcePipeline, Func<TSource,  Result<TResult>> selector)
        {
            Selector = selector;
            SourcePipeline = sourcePipeline;
        }
    }

    internal class Select<TData> : ISelect<TData>
    {
        public IPipeline SourcePipeline { get; }
        public Func<Result, Result<TData>> Selector { get; }

        public Select(IPipeline sourcePipeline, Func<Result, Result<TData>> selector)
        {
            Selector = selector;
            SourcePipeline = sourcePipeline;
        }
    }

    internal class Select : IPipeline
    {
        public readonly IPipeline SourcePipeline; 
        public readonly Func<Result, Result> Selector;
        
        public Select(IPipeline sourcePipeline, Func<Result, Result> selector)
        {
            Selector = selector;
            SourcePipeline = sourcePipeline;
        }
    }
}