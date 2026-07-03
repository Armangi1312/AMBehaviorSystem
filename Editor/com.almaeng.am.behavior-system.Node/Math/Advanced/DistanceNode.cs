using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Math.Advanced
{
    [Serializable]
    [NodeMenuItem("Math/Advanced/Distance")]
    public class DistanceNode : BaseDynamicTypeNode
    {
        public override string name => "Distance";

        protected override Type DefaultType => typeof(Vector2Port);

        [Input] public Port A;
        [Input] public Port B;

        [Output] public NumberPort Out;
    }
}
