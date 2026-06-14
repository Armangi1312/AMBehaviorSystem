using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Math.Basic
{
    [Serializable]
    [NodeMenuItem("Math/Basic/Square Root")]
    public class SquareRootNode : BaseChangeableMathNode
    {
        [Input] public NumberPort A;

        public override string name => "Square Root";
    }
}