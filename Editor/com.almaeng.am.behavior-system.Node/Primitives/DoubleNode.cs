using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Primitives
{
    [Serializable]
    [NodeMenuItem("Primitives/Double")]
    public class DoubleNode : BasePrimitiveNode<double, NumberPort> 
    {
        public override string name => "Double";
    }
}
