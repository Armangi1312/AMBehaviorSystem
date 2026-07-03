using AMBehaviorSystem.Node.Base;
using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Math.Advanced
{
    [Serializable]
    [NodeMenuItem("Math/Advanced/Lerp")]
    public class LerpNode : BaseDynamicTypeNode
    {
        public override string name => "Lerp";

        protected override Type DefaultType => typeof(NumberPort);

        [Input] public Port A;
        [Input] public Port B;
        [Input] public NumberPort T;

        [Output] public Port Out;
    }
}
