using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Math.Advanced
{
    [Serializable]
    [NodeMenuItem("Math/Advanced/Random")]
    public class RandomNode : BaseNode, IMathNode
    {
        public override string name => "Random";

        [Input] public NumberPort Min;
        [Input] public NumberPort Max;

        [Output] public NumberPort Out;
    }
}
