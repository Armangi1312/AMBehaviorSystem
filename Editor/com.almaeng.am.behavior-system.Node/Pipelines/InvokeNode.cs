using GraphProcessor;
using System;
using System.Collections.Generic;

namespace AMBehaviorSystem.Node.Pipelines
{
    [Serializable]
    [NodeMenuItem("Pipelines/Invoke")]
    public class InvokeNode : BasePipelineNode
    {
        public override string name => "Invoke";

        public List<string> ProcessorTypes = new();
    }
}
