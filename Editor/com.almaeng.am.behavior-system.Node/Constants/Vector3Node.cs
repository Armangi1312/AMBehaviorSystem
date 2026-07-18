using AMBehaviorSystem.Node.Ports;
using AMBehaviorSystem.Node.SourceGeneration;
using GraphProcessor;
using System;
using System.Globalization;
using UnityEngine;

namespace AMBehaviorSystem.Node.Constants
{
    [Serializable]
    [NodeMenuItem("Constant/Vector3")]
    public class Vector3Node : BaseValueNode<Vector3, Vector3Port>, IConstantNode
    {
        public override string name => "Vector3";

        protected override void Process()
        {
            SourceContext context = ((NodeGraph)graph).SourceContext;

            string value = $"new Vector3({Field.x.ToString(CultureInfo.InvariantCulture)}f, {Field.y.ToString(CultureInfo.InvariantCulture)}f, {Field.z.ToString(CultureInfo.InvariantCulture)}f)";
            string name = $"vector3_{GUIDParse.GetGUIDParse(GUID)}";
            Argument argument = new(typeof(Vector3), value);
            Expression expression = new(new[] { argument }, new[] { typeof(Vector3) }, typeof(Vector3), "#");
            DeclarationStatement statement = new(typeof(Vector3), name, expression);

            context.InvokeStatements.Add(statement);
            context.OutputLocals[GUID] = (typeof(Vector3), name);
        }
    }
}
