using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Math.Advanced
{
    [Serializable]
    [NodeMenuItem("Math/Advanced/Min")]
    public class MinNode : BaseNode, IMathNode
    {
        public override string name => "Min";

        [Input] public NumberPort A;
        [Input] public NumberPort B;

        [Output] public NumberPort Out;
    }
}
