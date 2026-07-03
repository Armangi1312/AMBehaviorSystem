using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Constants
{
    [Serializable]
    [NodeMenuItem("Constant/Double")]
    public class DoubleNode : BaseValueNode<double, NumberPort>, IConstantNode
    {
        public override string name => "Double";
    }
}
