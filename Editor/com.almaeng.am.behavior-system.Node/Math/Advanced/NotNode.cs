using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Math.Advanced
{
    [Serializable]
    [NodeMenuItem("Math/Advanced/Not")]
    public class NotNode : BaseNode, IMathNode
    {
        public override string name => "Not";

        [Input] public BooleanPort In;

        [Output] public BooleanPort Out;
    }
}
