using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Math.Advanced
{
    [Serializable]
    [NodeMenuItem("Math/Advanced/Abs")]
    public class AbsNode : BaseMathNode<NumberPort>
    {
        [Input] public NumberPort A;

        public override string name => "Abs";
    }
}