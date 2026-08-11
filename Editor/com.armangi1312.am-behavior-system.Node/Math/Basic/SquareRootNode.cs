using AMBehaviorSystem.Node.Ports;
using AMBehaviorSystem.Node.SourceGeneration.Context;
using AMBehaviorSystem.Node.SourceGeneration.Expressions;
using AMBehaviorSystem.Node.SourceGeneration.Statements;
using AMBehaviorSystem.Node.SourceGeneration.Traversal;
using AMBehaviorSystem.Node.SourceGeneration.Utilities;
using GraphProcessor;
using System;
using UnityEngine;

namespace AMBehaviorSystem.Node.Math.Basic
{
    [Serializable]
    [NodeMenuItem("Math/Basic/Square Root")]
    public class SquareRootNode : BaseNode, IMathNode, ISourceGenerationNode
    {
        public override string name => "Square Root";

        [Input] public NumberPort In;

        [Output] public NumberPort Out;

        public void Generate(SourceContext context)
        {
            (Type Type, string Name) input = NodeUtilities.GetInputVariable(nameof(In), context, this);

            string name = $"sqrt_{GUIDParse.GetGUIDParse(GUID)}";
            Type outType = input.Type == typeof(double) ? typeof(double) : typeof(float);

            Argument argument = new(input.Type, input.Name);

            string template = input.Type == typeof(double) ? "Math.Sqrt(#)" : "MathF.Sqrt(#)";

            ExpressionRule rule = new(template, outType, ArgumentConstraint.OfCategory(0, ArgumentCategory.Scalar));

            Expression expression = new(argument, rule);

            DeclarationStatement statement = new(outType, name, expression);

            context.InvokeStatements.Add(statement);
            context.OutputLocals[PortKey.Of(GUID, nameof(Out))] = (outType, name);
        }
    }
}