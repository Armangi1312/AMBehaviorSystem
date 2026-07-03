using AMBehaviorSystem.Node.Base;
using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Constants
{
    [Serializable]
    [NodeMenuItem("Constant/Boolean")]
    public class BooleanNode : BaseValueNode<bool, BooleanPort>, IConstantNode
    {
        public override string name => "Boolean";
    }
}
