using AMBehaviorSystem.Node.Ports;
using AMBehaviorSystem.Node.SourceGeneration;
using GraphProcessor;
using System;
using System.Globalization;
using UnityEngine;

namespace AMBehaviorSystem.Node.Constants
{
    [Serializable]
    [NodeMenuItem("Constant/Vector4")]
    public class Vector4Node : BaseValueNode<Vector4, Vector4Port>, IConstantNode
    {
        public override string name => "Vector4";

        protected override void Process()
        {
            SourceContext context = ((NodeGraph)graph).SourceContext;

            string value = $"new Vector4({Field.x.ToString(CultureInfo.InvariantCulture)}f, {Field.y.ToString(CultureInfo.InvariantCulture)}f, {Field.z.ToString(CultureInfo.InvariantCulture)}f)";
            string name = $"vector4_{GUIDParse.GetGUIDParse(GUID)}";
            Argument argument = new(typeof(Vector4), value);
            Expression expression = new(new[] { argument }, new[] { typeof(Vector4) }, typeof(Vector4), "#");
            DeclarationStatement statement = new(typeof(Vector4), name, expression);

            context.InvokeStatements.Add(statement);
            context.OutputLocals[GUID] = (typeof(Vector4), name);
        }
    }
}
