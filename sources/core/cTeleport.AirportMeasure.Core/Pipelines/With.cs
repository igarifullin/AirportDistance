using System;

namespace cTeleport.AirportMeasure.Core.Pipelines
{
    public interface IWith<in TData> : IPipeline<TData>
    {
    }
    
    internal class With<TData, TResult> : IInternalPipelineItem, IWith<TData>
    {
        internal readonly Func<TResult, IPipeline<TData>> ToFunc;
        internal readonly IPipeline<TResult> From;

        public With(IPipeline<TResult> from, Func<TResult, IPipeline<TData>> toFunc)
        {
            From = from;
            ToFunc = toFunc;
        }
    }

    internal class WithMultiple<TData, T1, T2> : IInternalPipelineItem, IWith<TData>
    {
        internal readonly Func<T1, T2, IPipeline<TData>> ToFunc;
        internal readonly IPipeline<T1> From1;
        internal readonly IPipeline<T2> From2;

        public WithMultiple(IPipeline<T1> from1, IPipeline<T2> from2, Func<T1, T2, IPipeline<TData>> toFunc)
        {
            From1 = from1;
            From2 = from2;
            ToFunc = toFunc;
        }
    }
}