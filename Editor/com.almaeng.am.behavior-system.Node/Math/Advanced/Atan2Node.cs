using AMBehaviorSystem.Node.Ports;
using AMBehaviorSystem.Node.SourceGeneration;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Math.Advanced
{
    [Serializable]
    [NodeMenuItem("Math/Advanced/Atan2")]
    public class Atan2Node : BaseNode, IMathNode
    {
        public override string name => "Atan2";

        [Input] public NumberPort Y;
        [Input] public NumberPort X;

        [Output] public NumberPort Out;

        protected override void Process()
        {
            SourceContext context = ((NodeGraph)graph).SourceContext;

            (Type Type, string Name) y = NodeUtilities.GetInputVariable(nameof(Y), context, this);
            (Type Type, string Name) x = NodeUtilities.GetInputVariable(nameof(X), context, this);

            string name = $"atan2_{GUIDParse.GetGUIDParse(GUID)}";
            Type outType = TypeUtilities.GetCastingType(y.Type, x.Type);

            Argument yArgument = new(y.Type, y.Name);
            Argument xArgument = new(x.Type, x.Name);

            string template = outType == typeof(double) ? "Math.Atan2(#, #)" : "MathF.Atan2(#, #)";

            ExpressionRule rule = new(template, outType,
                ArgumentConstraint.OfCategory(0, ArgumentCategory.Scalar),
                ArgumentConstraint.OfCategory(1, ArgumentCategory.Scalar),
                ArgumentConstraint.OfSameGroup(0, 1),
                ArgumentConstraint.OfSameGroup(1, 1));

            Expression expression = new(new[] { yArgument, xArgument }, rule);

            DeclarationStatement statement = new(outType, name, expression);

            context.InvokeStatements.Add(statement);
            context.OutputLocals[PortKey.Of(GUID, nameof(Out))] = (outType, name);
        }
    }
}