using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Pipelines
{
    [Serializable]
    [NodeMenuItem("Pipelines/Entry")]
    public class EntryNode : BaseNode
    {
        public override string name => "Entry";

        [Output] public PipelineFlowPort Entry;
    }
}
