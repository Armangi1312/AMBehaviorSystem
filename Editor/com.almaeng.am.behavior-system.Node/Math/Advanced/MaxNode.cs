using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Math.Advanced
{
    [Serializable]
    [NodeMenuItem("Math/Advanced/Max")]
    public class MaxNode : BaseNode, IMathNode
    {
        public override string name => "Max";

        [Input] public NumberPort A;
        [Input] public NumberPort B;

        [Output] public NumberPort Out;
    }
}
