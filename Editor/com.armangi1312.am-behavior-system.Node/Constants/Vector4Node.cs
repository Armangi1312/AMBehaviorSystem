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
    [NodeMenuItem("Constant/Vector4")]
    public class Vector4Node : BaseValueNode<Vector4, Vector4Port>, IConstantNode, ISourceGenerationNode
    {
        public override string name => "Vector4";

        public void Generate(SourceContext context)
        {
            string x = Field.x.ToString(CultureInfo.InvariantCulture);
            string y = Field.y.ToString(CultureInfo.InvariantCulture);
            string z = Field.z.ToString(CultureInfo.InvariantCulture);
            string w = Field.w.ToString(CultureInfo.InvariantCulture);

            string value = $"new Vector4({x}f, {y}f, {z}f, {w}f)";
            string name = $"vector4_{GUIDParse.GetGUIDParse(GUID)}";

            Argument argument = new(typeof(Vector4), value);
            ExpressionRule rule = new("#", typeof(Vector4), ArgumentConstraint.OfFixedType(0, typeof(Vector4)));

            Expression expression = new(argument, rule);
            DeclarationStatement statement = new(typeof(Vector4), name, expression);

            context.InvokeStatements.Add(statement);
            context.OutputLocals[PortKey.Of(GUID, nameof(Out))] = (typeof(Vector4), name);
        }
    }
}
