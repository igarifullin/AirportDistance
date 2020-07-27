using System;
using cTeleport.AirportMeasure.Core.Pipelines;

namespace cTeleport.AirportMeasure.Core.Extensions
{
    public static class PipelineExtensions
    {
        public static IPipeline<TData> And<TData>(this IPipeline from, IPipeline<TData> to) => new And<TData>(from, to);

        public static IPipeline<TData> With<TData, TResult>(this IPipeline<TResult> from, Func<TResult, IPipeline<TData>> toFunc) => new With<TData, TResult>(from, toFunc);

        public static IPipeline<TData> With<TData, T1, T2>(IPipeline<T1> from1, IPipeline<T2> from2,
            Func<T1, T2, IPipeline<TData>> toFunc) => new With<TData, T1, T2>(from1, from2, toFunc);

        public static IPipeline<TData> With<TData, T1, T2>(this IPipeline from, IPipeline<T1> pipe1,
            IPipeline<T2> pipe2,
            Func<T1, T2, IPipeline<TData>> func) =>
            new And<TData>(from, new With<TData, T1, T2>(pipe1, pipe2, func));
    }
}