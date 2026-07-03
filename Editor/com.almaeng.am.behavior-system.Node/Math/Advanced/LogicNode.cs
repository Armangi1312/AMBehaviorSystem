using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Math.Advanced
{
    [Serializable]
    [NodeMenuItem("Math/Advanced/Logic")]
    public class LogicNode : BaseNode, IMathNode
    {
        public override string name => "Logic";

        [Input] public BooleanPort A;
        [Input] public BooleanPort B;

        [Output] public BooleanPort Out;

        public LogicType Logic;

        public enum LogicType
        {
            And,
            Or,
            Xor
        }
    }
}
