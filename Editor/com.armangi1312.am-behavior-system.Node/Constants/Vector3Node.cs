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
    [NodeMenuItem("Constant/Vector3")]
    public class Vector3Node : BaseValueNode<Vector3, Vector3Port>, IConstantNode, ISourceGenerationNode
    {
        public override string name => "Vector3";

        public void Generate(SourceContext context)
        {
            string x = Field.x.ToString(CultureInfo.InvariantCulture);
            string y = Field.y.ToString(CultureInfo.InvariantCulture);
            string z = Field.z.ToString(CultureInfo.InvariantCulture);

            string value = $"new Vector3({x}f, {y}f, {z}f)";
            string name = $"vector3_{GUIDParse.GetGUIDParse(GUID)}";

            Argument argument = new(typeof(Vector3), value);
            ExpressionRule rule = new("#", typeof(Vector3), ArgumentConstraint.OfFixedType(0, typeof(Vector3)));

            Expression expression = new(argument, rule);
            DeclarationStatement statement = new(typeof(Vector3), name, expression);

            context.InvokeStatements.Add(statement);
            context.OutputLocals[PortKey.Of(GUID, nameof(Out))] = (typeof(Vector3), name);
        }
    }
}
