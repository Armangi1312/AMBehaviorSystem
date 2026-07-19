using AMBehaviorSystem.Node.Ports;
using AMBehaviorSystem.Node.SourceGeneration;
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

        protected override void Process()
        {
            SourceContext context = ((NodeGraph)graph).SourceContext;

            (Type Type, string Name) input = NodeUtilities.GetInputVariable(nameof(In), context, this);

            string name = $"floor_{GUIDParse.GetGUIDParse(GUID)}";
            Type outType = input.Type == typeof(double) ? typeof(double) : typeof(float);

            Argument argument = new(input.Type, input.Name);

            string template = input.Type == typeof(double) ? "Math.Floor(#)" : "MathF.Floor(#)";

            ExpressionRule rule = new(template, outType,
                ArgumentConstraint.OfCategory(0, ArgumentCategory.Float));

            Expression expression = new(argument, rule);

            DeclarationStatement statement = new(outType, name, expression);

            context.InvokeStatements.Add(statement);
            context.OutputLocals[GUID] = (outType, name);
        }
    }
}