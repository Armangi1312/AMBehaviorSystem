using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;
using System.Collections.Generic;

namespace AMBehaviorSystem.Node.Pipelines
{
    [Serializable]
    public abstract class BasePipelineNode : BaseNode
    {
        [Input] public PipelineFlowPort In;
        [Output] public PipelineFlowPort Next;
    }

    [Serializable]
    [NodeMenuItem("Pipelines/Entry")]
    public class EntryNode : BaseNode
    {
        public override string name => "Entry";

        [Output] public PipelineFlowPort Entry;
    }

    [Serializable]
    [NodeMenuItem("Pipelines/Condition")]
    public class ConditionNode : BasePipelineNode
    {
        public override string name => "Condition";

        [Input] public BooleanPort Condition;

        [Output] public PipelineFlowPort True;
        [Output] public PipelineFlowPort False;
    }

    [Serializable]
    [NodeMenuItem("Pipelines/Invoke")]
    public class InvokeNode : BasePipelineNode
    {
        public override string name => "Invoke";

        public List<string> ProcessorTypes = new();
    }
}
