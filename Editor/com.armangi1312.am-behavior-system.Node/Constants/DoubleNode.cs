using AMBehaviorSystem.Node.Ports;
using AMBehaviorSystem.Node.SourceGeneration.Context;
using AMBehaviorSystem.Node.SourceGeneration.Expressions;
using AMBehaviorSystem.Node.SourceGeneration.Statements;
using AMBehaviorSystem.Node.SourceGeneration.Traversal;
using AMBehaviorSystem.Node.SourceGeneration.Utilities;
using GraphProcessor;
using System;
using System.Globalization;

namespace AMBehaviorSystem.Node.Constants
{
    [Serializable]
    [NodeMenuItem("Constant/Double")]
    public class DoubleNode : BaseValueNode<double, NumberPort>, IConstantNode, ISourceGenerationNode
    {
        public override string name => "Double";

        public void Generate(SourceContext context)
        {
            string value = $"{Field.ToString(CultureInfo.InvariantCulture)}d";
            string name = $"double_{GUIDParse.GetGUIDParse(GUID)}";

            Argument argument = new(typeof(double), value);
            ExpressionRule rule = new("#", typeof(double), ArgumentConstraint.OfFixedType(0, typeof(double)));

            Expression expression = new(argument, rule);
            DeclarationStatement statement = new(typeof(double), name, expression);

            context.InvokeStatements.Add(statement);
            context.OutputLocals[PortKey.Of(GUID, nameof(Out))] = (typeof(double), name);
        }
    }
}
