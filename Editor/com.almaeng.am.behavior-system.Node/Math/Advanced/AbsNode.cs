using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Math.Advanced
{
    [Serializable]
    [NodeMenuItem("Math/Advanced/Abs")]
    public class AbsNode : BaseNode, IMathNode
    {
        public override string name => "Abs";

        [Input] public NumberPort In;

        [Output] public NumberPort Out;
    }
}
