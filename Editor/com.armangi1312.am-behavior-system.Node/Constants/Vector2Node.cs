using AMBehaviorSystem.Node.Ports;
using AMBehaviorSystem.Node.SourceGeneration.Context;
using AMBehaviorSystem.Node.SourceGeneration.Expressions;
using AMBehaviorSystem.Node.SourceGeneration.Statements;
using AMBehaviorSystem.Node.SourceGeneration.Traversal;
using AMBehaviorSystem.Node.SourceGeneration.Utilities;
using GraphProcessor;
using System;
using System.Globalization;
using UnityEngine;

namespace AMBehaviorSystem.Node.Constants
{
    [Serializable]
    [NodeMenuItem("Constant/Vector2")]
    public class Vector2Node : BaseValueNode<Vector2, Vector2Port>, IConstantNode, ISourceGenerationNode
    {
        public override string name => "Vector2";

        public void Generate(SourceContext context)
        {
            string x = Field.x.ToString(CultureInfo.InvariantCulture);
            string y = Field.y.ToString(CultureInfo.InvariantCulture);

            string value = $"new Vector2({x}f, {y}f)";
            string name = $"vector2_{GUIDParse.GetGUIDParse(GUID)}";

            Argument argument = new(typeof(Vector2), value);
            ExpressionRule rule = new("#", typeof(Vector2), ArgumentConstraint.OfFixedType(0, typeof(Vector2)));

            Expression expression = new(argument, rule);
            DeclarationStatement statement = new(typeof(Vector2), name, expression);

            context.InvokeStatements.Add(statement);
            context.OutputLocals[PortKey.Of(GUID, nameof(Out))] = (typeof(Vector2), name);
        }
    }
}
