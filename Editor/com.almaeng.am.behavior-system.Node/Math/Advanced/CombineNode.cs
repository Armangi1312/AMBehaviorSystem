using AMBehaviorSystem.Node.Ports;
using AMBehaviorSystem.Node.SourceGeneration.Context;
using AMBehaviorSystem.Node.SourceGeneration.Expressions;
using AMBehaviorSystem.Node.SourceGeneration.Statements;
using AMBehaviorSystem.Node.SourceGeneration.Utilities;
using GraphProcessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace AMBehaviorSystem.Node.Math.Advanced
{
    [Serializable]
    [NodeMenuItem("Math/Advanced/Combine")]
    public class CombineNode : BaseNode
    {
        public override string name => "Combine";

        private const int ComponentIndexX = 0;
        private const int ComponentIndexY = 1;
        private const int ComponentIndexZ = 2;
        private const int ComponentIndexW = 3;

        private readonly HashSet<string> connectedFields = new();

        [Input] public NumberPort X;
        [Input] public NumberPort Y;
        [Input] public NumberPort Z;
        [Input] public NumberPort W;

        [Output] public Port Out;

        protected override void Process()
        {
            SourceContext context = ((NodeGraph)graph).SourceContext;

            Type outputType = GetOutputType() switch
            {
                Type type when type == typeof(Vector4Port) => typeof(Vector4),
                Type type when type == typeof(Vector3Port) => typeof(Vector3),
                _ => typeof(Vector2)
            };

            string[] axisNames = { "x", "y", "z", "w" };
            string[] fieldNames = { nameof(X), nameof(Y), nameof(Z), nameof(W) };
            int componentCount = outputType == typeof(Vector4) ? 4 : outputType == typeof(Vector3) ? 3 : 2;

            List<string> componentExpressions = new();

            for (int i = 0; i < componentCount; i++)
            {
                if (connectedFields.Contains(fieldNames[i]))
                {
                    (Type Type, string Name) input = NodeUtilities.GetInputVariable(fieldNames[i], context, this);

                    Argument argument = new(input.Type, input.Name);
                    ExpressionRule rule = new("#", typeof(float), ArgumentConstraint.OfCategory(0, ArgumentCategory.Scalar));

                    Expression expression = new(argument, rule);
                    componentExpressions.Add(expression.ToString());
                }
                else
                {
                    componentExpressions.Add("0f");
                }
            }

            string name = $"combine_{GUIDParse.GetGUIDParse(GUID)}";
            string template = $"new {outputType.Name}({string.Join(", ", componentExpressions)})";

            Argument dummyArgument = new(outputType, template);
            ExpressionRule dummyRule = new("#", outputType, ArgumentConstraint.OfFixedType(0, outputType));

            Expression finalExpression = new(dummyArgument, dummyRule);

            DeclarationStatement statement = new(outputType, name, finalExpression);

            context.InvokeStatements.Add(statement);
            context.OutputLocals[PortKey.Of(GUID, nameof(Out))] = (outputType, name);
        }

        public override IEnumerable<FieldInfo> OverrideFieldOrder(IEnumerable<FieldInfo> fields)
        {
            string[] order = { nameof(X), nameof(Y), nameof(Z), nameof(W), nameof(Out) };
            return fields.OrderByDescending(f => Array.IndexOf(order, f.Name));
        }

        protected override void Enable()
        {
            onAfterEdgeConnected -= HandleEdgeConnected;
            onAfterEdgeConnected += HandleEdgeConnected;

            onAfterEdgeDisconnected -= HandleEdgeDisconnected;
            onAfterEdgeDisconnected += HandleEdgeDisconnected;
        }

        private void HandleEdgeConnected(SerializableEdge edge)
        {
            if (edge.inputNode != this) return;

            connectedFields.Add(edge.inputFieldName);
            UpdateAllPorts();
        }

        private void HandleEdgeDisconnected(SerializableEdge edge)
        {
            if (edge.inputNode != this) return;

            connectedFields.Remove(edge.inputFieldName);
            UpdateAllPorts();
        }

        private Type GetOutputType()
        {
            int maxIndex = ComponentIndexX;

            if (connectedFields.Contains(nameof(X))) maxIndex = System.Math.Max(maxIndex, ComponentIndexX);
            if (connectedFields.Contains(nameof(Y))) maxIndex = System.Math.Max(maxIndex, ComponentIndexY);
            if (connectedFields.Contains(nameof(Z))) maxIndex = System.Math.Max(maxIndex, ComponentIndexZ);
            if (connectedFields.Contains(nameof(W))) maxIndex = System.Math.Max(maxIndex, ComponentIndexW);

            if (maxIndex >= ComponentIndexW) return typeof(Vector4Port);
            if (maxIndex >= ComponentIndexZ) return typeof(Vector3Port);

            return typeof(Vector2Port);
        }

        [CustomPortBehavior(nameof(Out))]
        private IEnumerable<PortData> OutPortBehavior(List<SerializableEdge> edges)
        {
            yield return new PortData { identifier = nameof(Out), displayName = nameof(Out), displayType = GetOutputType() };
        }
    }
}