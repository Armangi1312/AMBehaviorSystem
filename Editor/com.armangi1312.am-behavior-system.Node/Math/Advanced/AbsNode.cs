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
    [NodeMenuItem("Math/Advanced/Abs")]
    public class AbsNode : BaseNode, IMathNode, ISourceGenerationNode
    {
        public override string name => "Abs";

        [Input] public NumberPort In;

        [Output] public NumberPort Out;

        public void Generate(SourceContext context)
        {
            (Type Type, string Name) input = NodeUtilities.GetInputVariable(nameof(In), context, this);

            string name = $"abs_{GUIDParse.GetGUIDParse(GUID)}";
            Type outType = input.Type;

            Argument argument = new(input.Type, input.Name);

            ExpressionRule rule = new("Math.Abs(#)", outType, ArgumentConstraint.OfCategory(0, ArgumentCategory.Scalar));

            Expression expression = new(argument, rule);

            DeclarationStatement statement = new(outType, name, expression);

            context.InvokeStatements.Add(statement);
            context.OutputLocals[PortKey.Of(GUID, nameof(Out))] = (outType, name);
        }
    }
}