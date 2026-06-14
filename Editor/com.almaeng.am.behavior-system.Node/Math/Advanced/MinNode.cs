using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Math.Advanced
{
    [Serializable]
    [NodeMenuItem("Math/Advanced/Min")]
    public class MinNode : BaseMathNode<NumberPort>
    {
        [Input] public NumberPort A;
        [Input] public NumberPort B;

        public override string name => "Min";
    }
}