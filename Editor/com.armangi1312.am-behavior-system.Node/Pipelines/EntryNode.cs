using AMBehaviorSystem.Node.Ports;
using AMBehaviorSystem.Node.SourceGeneration.Traversal;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Pipelines
{
    [Serializable]
    [NodeMenuItem("Pipelines/Entry")]
    public class EntryNode : BaseNode, INonGenerativeNode
    {
        public override string name => "Entry";

        [Output] public PipelineFlowPort Entry;
    }
}
