using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Math.Advanced
{
    [Serializable]
    [NodeMenuItem("Math/Advanced/Sin")]
    public class SinNode : BaseNode, IMathNode
    {
        public override string name => "Sin";
        [Input] public NumberPort In;

        [Output] public NumberPort Out;
    }
}
