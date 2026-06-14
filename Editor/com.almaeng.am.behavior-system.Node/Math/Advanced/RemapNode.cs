using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Math.Advanced
{
    [Serializable]
    [NodeMenuItem("Math/Advanced/Remap")]
    public class RemapNode : BaseMathNode<NumberPort>
    {
        [Input] public NumberPort Value;
        [Input] public NumberPort FromMin;
        [Input] public NumberPort FromMax;

        [Input] public NumberPort ToMin;
        [Input] public NumberPort ToMax;

        public override string name => "Remap";
    }
}