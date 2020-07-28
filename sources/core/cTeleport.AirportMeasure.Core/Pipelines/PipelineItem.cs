using System;
using cTeleport.AirportMeasure.Core.Results;

namespace cTeleport.AirportMeasure.Core.Pipelines
{
    public interface IPipelineItem
    {
        Type In { get; }
        Type Out { get; }

        Result<object> Execute();
    }
    
    public class PipelineItem<TOut> : IPipelineItem
    {
        private readonly Func<Result<TOut>> _action;

        public PipelineItem(Func<Result<TOut>> action)
        {
            Out = typeof(TOut);
            _action = action;
        }

        public Result<object> Execute()
        {
            return _action();
        }

        public Type In { get; }
        public Type Out { get; }
    }

    public class PipelineItem<TIn, TOut> : IPipelineItem
    {
        private readonly Func<TIn, Result<TOut>> _action;
        private readonly TIn _input;

        public PipelineItem(Func<TIn, Result<TOut>> action, TIn input)
        {
            In = typeof(TIn);
            Out = typeof(TOut);
            _action = action;
            _input = input;
        }

        public Result<object> Execute()
        {
            return _action(_input);
        }

        public Type In { get; }
        public Type Out { get; }
    }
}