using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Math.Advanced
{
    [Serializable]
    [NodeMenuItem("Math/Advanced/Clamp")]
    public class ClampNode : BaseMathNode<NumberPort>
    {
        [Input] public NumberPort In;
        [Input] public NumberPort Min;
        [Input] public NumberPort Max;

        public override string name => "Clamp";
    }
}