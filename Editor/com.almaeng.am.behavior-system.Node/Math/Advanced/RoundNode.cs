using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Math.Advanced
{
    [Serializable]
    [NodeMenuItem("Math/Advanced/Round")]
    public class RoundNode : BaseMathNode<NumberPort>
    {
        [Input] public NumberPort In;

        public override string name => "Round";
    }
}