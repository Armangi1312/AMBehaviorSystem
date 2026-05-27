using System.Collections.Generic;

namespace AMBehaviorSystem.Core.Pipelines
{
    public class InvokePipeline : Pipeline
    {
        public override void Execute(IReadOnlyList<Processor> processors, InvokeTiming timing)
        {
            for (int i = 0; i < processors.Count; i++)
            {
                var processor = processors[i];

                if ((processor.InvokeTiming & timing) != 0)
                    processor.Process();
            }

            NextPipeline?.Execute(processors, timing);
        }

        public InvokePipeline(Pipeline nextPipeline) : base(nextPipeline)
        {
        }
    }
}
