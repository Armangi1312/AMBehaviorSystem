using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Math.Advanced
{
    [Serializable]
    [NodeMenuItem("Math/Advanced/Ceil")]
    public class CeilNode : BaseNode, IMathNode
    {
        public override string name => "Ceil";

        [Input] public NumberPort In;

        [Output] public NumberPort Out;
    }
}
