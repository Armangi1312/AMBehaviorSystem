using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Math.Advanced
{
    [Serializable]
    [NodeMenuItem("Math/Advanced/Tan")]
    public class TanNode : BaseNode, IMathNode
    {
        public override string name => "Tan";

        [Input] public NumberPort In;

        [Output] public NumberPort Out;
    }
}
