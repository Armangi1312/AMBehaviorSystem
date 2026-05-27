using System;
using System.Collections.Generic;
using System.Text;

namespace AMBehaviorSystem.Core.Pipelines
{
    public class ConditionPipeline : Pipeline
    {
        public readonly (Func<bool> Condition, Pipeline Next)[] Conditions;

        public override void Execute(IReadOnlyList<Processor> processors, InvokeTiming timing)
        {
            for(int i = 0; i < Conditions.Length; i++)
            {
                var (condition, nextPipeline) = Conditions[i];

                if (condition())
                {
                    nextPipeline.Execute(processors, timing);
                    break;
                }
            }

            NextPipeline?.Execute(processors, timing);
        }

        public ConditionPipeline(Pipeline nextPipeline, (Func<bool> Condition, Pipeline Next)[] conditions) : base(nextPipeline)
        {
            Conditions = conditions;
        }
    }
}
