using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Math.Advanced
{
    [Serializable]
    [NodeMenuItem("Math/Advanced/Normalize")]
    public class NormalizeNode : BaseDynamicTypeNode
    {
        public override string name => "Normalize";

        protected override Type DefaultType => typeof(Vector2Port);

        [Input] public Port In;
        [Output] public Port Out;
    }
}
