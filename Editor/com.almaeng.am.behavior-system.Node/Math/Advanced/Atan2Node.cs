using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;
using System.Collections.Generic;

namespace AMBehaviorSystem.Node.Math.Advanced
{
    [Serializable]
    [NodeMenuItem("Math/Advanced/Atan2")]
    public class Atan2Node : BaseChangeableMathNode<NumberPort>
    {
        [Input] public NumberPort Y;

        [Input] public NumberPort X;

        public override string name => "Atan2";
    }
}