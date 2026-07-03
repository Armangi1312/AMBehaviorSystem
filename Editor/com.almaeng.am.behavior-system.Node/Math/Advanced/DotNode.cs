using AMBehaviorSystem.Node.Base;
using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Math.Advanced
{
    [Serializable]
    [NodeMenuItem("Math/Advanced/Dot")]
    public class DotNode : BaseDynamicTypeNode
    {
        public override string name => "Dot";

        protected override Type DefaultType => typeof(Vector2Port);

        [Input] public Port A;
        [Input] public Port B;

        [Output] public NumberPort Out;
    }
}
