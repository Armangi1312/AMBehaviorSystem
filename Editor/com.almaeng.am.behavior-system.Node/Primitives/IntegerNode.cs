using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Primitives
{
    [Serializable]
    [NodeMenuItem("Primitives/Integer")]
    public class IntegerNode : BasePrimitiveNode<int, NumberPort> 
    {
        public override string name => "Integer";
    }
}
