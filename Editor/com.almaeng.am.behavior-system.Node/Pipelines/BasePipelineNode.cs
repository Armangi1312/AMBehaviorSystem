using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Pipelines
{
    [Serializable]
    public class BasePipelineNode : BaseNode
    {
        [Input] public PipelineFlowPort In;
        [Output] public PipelineFlowPort Next;
    }
}
