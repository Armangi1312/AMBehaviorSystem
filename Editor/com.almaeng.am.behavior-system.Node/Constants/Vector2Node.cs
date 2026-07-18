using AMBehaviorSystem.Node.Ports;
using AMBehaviorSystem.Node.SourceGeneration;
using GraphProcessor;
using System;
using System.Globalization;
using UnityEngine;

namespace AMBehaviorSystem.Node.Constants
{
    [Serializable]
    [NodeMenuItem("Constant/Vector2")]
    public class Vector2Node : BaseValueNode<Vector2, Vector2Port>, IConstantNode
    {
        public override string name => "Vector2";

        protected override void Process()
        {
            SourceContext context = ((NodeGraph)graph).SourceContext;

            string value = $"new Vector2({Field.x.ToString(CultureInfo.InvariantCulture)}f, {Field.y.ToString(CultureInfo.InvariantCulture)}f)";
            string name = $"vector2_{GUIDParse.GetGUIDParse(GUID)}";
            Argument argument = new(typeof(Vector2), value);
            Expression expression = new(new[] { argument }, new[] { typeof(Vector2) }, typeof(Vector2), "#");
            DeclarationStatement statement = new(typeof(Vector2), name, expression);

            context.InvokeStatements.Add(statement);
            context.OutputLocals[GUID] = (typeof(Vector2), name);
        }
    }
}
