using AMBehaviorSystem.Node.Base;
using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Constants
{
    [Serializable]
    [NodeMenuItem("Constant/Float")]
    public class FloatNode : BaseValueNode<float, NumberPort>, IConstantNode
    {
        public override string name => "Float";
    }
}
