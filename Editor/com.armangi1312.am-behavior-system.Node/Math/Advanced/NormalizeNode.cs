using AMBehaviorSystem.Node.Ports;
using AMBehaviorSystem.Node.SourceGeneration.Context;
using AMBehaviorSystem.Node.SourceGeneration.Expressions;
using AMBehaviorSystem.Node.SourceGeneration.Statements;
using AMBehaviorSystem.Node.SourceGeneration.Traversal;
using AMBehaviorSystem.Node.SourceGeneration.Utilities;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Math.Advanced
{
    [Serializable]
    [NodeMenuItem("Math/Advanced/Normalize")]
    public class NormalizeNode : BaseDynamicTypeNode, ISourceGenerationNode
    {
        public override string name => "Normalize";

        protected override Type DefaultType => typeof(Vector2Port);

        [Input] public Port In;
        [Output] public Port Out;

        public void Generate(SourceContext context)
        {
            (Type Type, string Name) input = NodeUtilities.GetInputVariable(nameof(In), context, this);

            string name = $"normalize_{GUIDParse.GetGUIDParse(GUID)}";
            Type outType = input.Type;

            Argument argument = new(input.Type, input.Name);

            ExpressionRule rule = new("#.normalized", outType,
                ArgumentConstraint.OfCategory(0, ArgumentCategory.Vector));

            Expression expression = new(argument, rule);

            DeclarationStatement statement = new(outType, name, expression);

            context.InvokeStatements.Add(statement);
            context.OutputLocals[PortKey.Of(GUID, nameof(Out))] = (outType, name);
        }
    }
}