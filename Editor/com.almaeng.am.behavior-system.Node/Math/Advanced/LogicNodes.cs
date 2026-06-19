using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Math.Advanced
{
    public enum ComparisonType
    {
        Equal,
        NotEqual,
        Less,
        LessOrEqual,
        Greater,
        GreaterOrEqual,
    }

    public enum LogicType
    {
        And,
        Or,
        Xor,
        Not
    }

    [Serializable]
    [NodeMenuItem("Math/Advanced/Comparison")]
    public class ComparisonNode : BaseMathNode<BooleanPort>
    {
        public override string name => "Comparison";

        [Input] public NumberPort A;
        [Input] public NumberPort B;

        public ComparisonType ComparisonType;
    }

    [Serializable]
    [NodeMenuItem("Math/Advanced/Logic")]
    public class LogicNode : BaseMathNode<BooleanPort>
    {
        public override string name => "Logic";

        [Input] public BooleanPort A;
        [Input] public BooleanPort B;

        public LogicType LogicType;
    }
}
