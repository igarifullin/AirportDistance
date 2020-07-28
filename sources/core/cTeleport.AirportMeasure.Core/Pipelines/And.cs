using System;
using System.Collections.Generic;
using System.Linq;

namespace cTeleport.AirportMeasure.Core.Pipelines
{
    /// <summary>
    /// Класс прохождения этапов друг за другом
    /// </summary>
    internal class And : IInternalPipelineItem, IPipeline
    {
        internal readonly List<IPipeline> Pipelines = new List<IPipeline>();

        /// <summary>
        /// Возвращает все имеющиеся этапы. Необходмио для внутреннего join-a
        /// </summary>
        protected virtual IEnumerable<IPipeline> AllPipelines => Pipelines;

        public And(IPipeline from, IPipeline to)
        {
            if (from is And and)
            {
                // Join-им несколько And в один, дабы не вызывать кучу раз 
                Pipelines.AddRange(and.AllPipelines);
            }
            else
            {
                Pipelines.Add(from);
            }
            
            if (to is And andTo)
            {
                Pipelines.AddRange(andTo.AllPipelines);
            }
            else
            {
                if (to != null)
                {                    
                    Pipelines.Add(to);
                }
            }
        }
    }

    internal class And<TData> : And, IPipeline<TData>
    {
        internal readonly IPipeline<TData> Last;

        public And(IPipeline from, IPipeline<TData> to) : base(from, null)
        {
            if (Last != null)
            {
                Pipelines.Add(Last);
            }
            
            Last = to;
        }

        protected override IEnumerable<IPipeline> AllPipelines
        {
            get
            {
                var result = Pipelines.ToList();
                result.Add(Last);
                return result;
            }
        }
    }

    internal class AndCreator : IPipeline
    {
        internal readonly List<Func<IPipeline>> Pipelines = new List<Func<IPipeline>>();

        /// <summary>
        /// Возвращает все имеющиеся этапы. Необходмио для внутреннего join-a
        /// </summary>
        protected virtual IEnumerable<Func<IPipeline>> AllPipelines => Pipelines;
    }

    internal class AndCreator<TData, TResult> : IPipeline<TData>
    {
        internal readonly Func<TResult, IPipeline<TData>> ToFunc;
        internal readonly IPipeline<TResult> From;

        public AndCreator(IPipeline<TResult> from, Func<TResult, IPipeline<TData>> toFunc)
        {
            From = from;
            ToFunc = toFunc;
        }
    }
}