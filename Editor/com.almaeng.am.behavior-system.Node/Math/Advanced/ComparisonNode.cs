using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Math.Advanced
{
    [Serializable]
    [NodeMenuItem("Math/Advanced/Comparison")]
    public class ComparisonNode : BaseNode, IMathNode
    {
        public override string name => "Comparison";

        [Input] public NumberPort A;
        [Input] public NumberPort B;

        [Output] public BooleanPort Out;

        public ComparisonType Comparison;

        public enum ComparisonType
        {
            Equal,
            NotEqual,
            Less,
            LessOrEqual,
            Greater,
            GreaterOrEqual,
        }
    }
}
