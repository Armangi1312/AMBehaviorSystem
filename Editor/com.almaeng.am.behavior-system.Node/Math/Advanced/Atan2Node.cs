using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Math.Advanced
{
    [Serializable]
    [NodeMenuItem("Math/Advanced/Atan2")]
    public class Atan2Node : BaseMathNode<NumberPort>
    {
        [Input] public NumberPort Y;

        [Input] public NumberPort X;

        public override string name => "Atan2";
    }
}