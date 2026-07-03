using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Math.Advanced
{
    [Serializable]
    [NodeMenuItem("Math/Advanced/Cos")]
    public class CosNode : BaseNode, IMathNode
    {
        public override string name => "Cos";

        [Input] public NumberPort In;

        [Output] public NumberPort Out;
    }
}
