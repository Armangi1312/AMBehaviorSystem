using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Primitives
{
    [Serializable]
    [NodeMenuItem("Primitives/Boolean")]
    public class BooleanNode : BasePrimitiveNode<bool, BooleanPort>
    {
        public override string name => "Boolean";        
    }
}
