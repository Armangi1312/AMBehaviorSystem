using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Primitives
{
    [Serializable]
    [NodeMenuItem("Primitives/Float")]
    public class FloatNode : BasePrimitiveNode<float, NumberPort> 
    {
        public override string name => "Float";
    }
}
