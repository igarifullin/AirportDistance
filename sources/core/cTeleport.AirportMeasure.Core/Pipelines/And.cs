using System.Collections.Generic;
using System.Linq;

namespace cTeleport.AirportMeasure.Core.Pipelines
{
    internal class And : IInternalPipelineItem, IPipeline
    {
        internal readonly List<IPipeline> Pipelines = new List<IPipeline>();

        protected virtual IEnumerable<IPipeline> AllPipelines => Pipelines;

        public And(IPipeline from, IPipeline to)
        {
            if (from is And and)
            {
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
}