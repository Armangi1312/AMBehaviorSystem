using AMBehaviorSystem.Node.Ports;
using AMBehaviorSystem.Node.SourceGeneration.Context;
using AMBehaviorSystem.Node.SourceGeneration.Expressions;
using AMBehaviorSystem.Node.SourceGeneration.Statements;
using AMBehaviorSystem.Node.SourceGeneration.Utilities;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Math.Advanced
{
    [Serializable]
    [NodeMenuItem("Math/Advanced/Magnitude")]
    public class MagnitudeNode : BaseDynamicTypeNode
    {
        public override string name => "Magnitude";

        protected override Type DefaultType => typeof(Vector2Port);

        [Input] public Port In;

        [Output] public NumberPort Out;

        protected override void Process()
        {
            SourceContext context = ((NodeGraph)graph).SourceContext;

            (Type Type, string Name) input = NodeUtilities.GetInputVariable(nameof(In), context, this);

            string name = $"magnitude_{GUIDParse.GetGUIDParse(GUID)}";
            Type outType = typeof(float);

            Argument argument = new(input.Type, input.Name);

            ExpressionRule rule = new("#.magnitude", outType,
                ArgumentConstraint.OfCategory(0, ArgumentCategory.Vector));

            Expression expression = new(argument, rule);

            DeclarationStatement statement = new(outType, name, expression);

            context.InvokeStatements.Add(statement);
            context.OutputLocals[PortKey.Of(GUID, nameof(Out))] = (outType, name);
        }
    }
}