using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Math.Advanced
{
    [Serializable]
    [NodeMenuItem("Math/Advanced/Magnitude")]
    public class MagnitudeNode : BaseDynamicTypeNode
    {
        public override string name => "Magnitude";

        protected override Type DefaultType => typeof(Vector2Port);

        [Input] public Port In;

        [Output] public NumberPort Out;
    }
}
