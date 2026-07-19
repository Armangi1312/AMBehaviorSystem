using AMBehaviorSystem.Node.Ports;
using AMBehaviorSystem.Node.SourceGeneration;
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

        protected override void Process()
        {
            SourceContext context = ((NodeGraph)graph).SourceContext;

            (Type Type, string Name) a = NodeUtilities.GetInputVariable(nameof(A), context, this);
            (Type Type, string Name) b = NodeUtilities.GetInputVariable(nameof(B), context, this);

            string name = $"comparison_{GUIDParse.GetGUIDParse(GUID)}";
            Type outType = typeof(bool);

            Argument leftArgument = new(a.Type, a.Name);
            Argument rightArgument = new(b.Type, b.Name);

            string template = GetTemplate(Comparison);

            ExpressionRule rule = new(template, outType,
                ArgumentConstraint.OfCategory(0, ArgumentCategory.Scalar),
                ArgumentConstraint.OfCategory(1, ArgumentCategory.Scalar),
                ArgumentConstraint.OfSameGroup(0, 1),
                ArgumentConstraint.OfSameGroup(1, 1));

            Expression expression = new(new[] { leftArgument, rightArgument }, rule);

            DeclarationStatement statement = new(outType, name, expression);

            context.InvokeStatements.Add(statement);
            context.OutputLocals[GUID] = (outType, name);
        }

        private static string GetTemplate(ComparisonType comparison)
        {
            return comparison switch
            {
                ComparisonType.Equal => "# == #",
                ComparisonType.NotEqual => "# != #",
                ComparisonType.Less => "# < #",
                ComparisonType.LessOrEqual => "# <= #",
                ComparisonType.Greater => "# > #",
                ComparisonType.GreaterOrEqual => "# >= #",
                _ => throw new ArgumentOutOfRangeException(nameof(comparison), comparison, null)
            };
        }
    }
}