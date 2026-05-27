using System;
using System.Collections.Generic;

namespace AMBehaviorSystem.Core.Pipelines
{
    public abstract class Pipeline
    {
        public readonly Pipeline NextPipeline;

        public abstract void Execute(IReadOnlyList<Processor> processors, InvokeTiming timing);

        protected Pipeline(Pipeline nextPipeline)
        {
            NextPipeline = nextPipeline;
        }
    }
}
