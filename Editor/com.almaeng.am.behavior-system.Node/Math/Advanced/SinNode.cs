using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Math.Advanced
{
    [Serializable]
    [NodeMenuItem("Math/Advanced/Sin")]
    public class SinNode : BaseMathNode<NumberPort>
    {
        [Input] public NumberPort In;

        public override string name => "Sin";
    }
}