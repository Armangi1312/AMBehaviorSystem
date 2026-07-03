using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Constants
{
    [Serializable]
    [NodeMenuItem("Constant/Integer")]
    public class IntegerNode : BaseValueNode<int, NumberPort>, IConstantNode
    {
        public override string name => "Integer";
    }
}
