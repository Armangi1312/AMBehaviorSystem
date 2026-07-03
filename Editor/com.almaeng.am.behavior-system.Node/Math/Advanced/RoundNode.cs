using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Math.Advanced
{
    [Serializable]
    [NodeMenuItem("Math/Advanced/Round")]
    public class RoundNode : BaseNode, IMathNode
    {
        public override string name => "Round";

        [Input] public NumberPort In;

        [Output] public NumberPort Out;
    }
}
