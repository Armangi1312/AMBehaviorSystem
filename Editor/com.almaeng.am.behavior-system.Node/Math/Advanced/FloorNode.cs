using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Math.Advanced
{
    [Serializable]
    [NodeMenuItem("Math/Advanced/Floor")]
    public class FloorNode : BaseNode, IMathNode
    {
        public override string name => "Floor";

        [Input] public NumberPort In;

        [Output] public NumberPort Out;
    }
}
