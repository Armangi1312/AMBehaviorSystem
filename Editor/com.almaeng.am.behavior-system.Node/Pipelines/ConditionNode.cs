using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Pipelines
{
    [Serializable]
    [NodeMenuItem("Pipelines/Condition")]
    public class ConditionNode : BasePipelineNode
    {
        public override string name => "Condition";

        [Input] public BooleanPort Condition;

        [Output] public PipelineFlowPort True;
        [Output] public PipelineFlowPort False;
    }
}