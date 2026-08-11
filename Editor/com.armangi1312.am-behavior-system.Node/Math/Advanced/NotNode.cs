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
    [NodeMenuItem("Math/Advanced/Not")]
    public class NotNode : BaseNode, IMathNode, ISourceGenerationNode
    {
        public override string name => "Not";

        [Input] public BooleanPort In;

        [Output] public BooleanPort Out;

        public void Generate(SourceContext context)
        {
            (Type Type, string Name) input = NodeUtilities.GetInputVariable(nameof(In), context, this);

            string name = $"not_{GUIDParse.GetGUIDParse(GUID)}";
            Type outType = typeof(bool);

            Argument argument = new(input.Type, input.Name);

            ExpressionRule rule = new("!#", outType,
                ArgumentConstraint.OfFixedType(0, typeof(bool)));

            Expression expression = new(argument, rule);

            DeclarationStatement statement = new(outType, name, expression);

            context.InvokeStatements.Add(statement);
            context.OutputLocals[PortKey.Of(GUID, nameof(Out))] = (outType, name);
        }
    }
}