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
    [NodeMenuItem("Math/Advanced/Split")]
    public class SplitNode : BaseDynamicTypeNode
    {
        public override string name => "Split";

        protected override Type DefaultType => typeof(Vector2Port);

        private const int ComponentCountX = 1;
        private const int ComponentCountY = 2;
        private const int ComponentCountZ = 3;
        private const int ComponentCountW = 4;

        [Input] public Port In;

        [Output] public NumberPort X;
        [Output] public NumberPort Y;
        [Output] public NumberPort Z;
        [Output] public NumberPort W;

        protected override void Process()
        {
            SourceContext context = ((NodeGraph)graph).SourceContext;

            (Type Type, string Name) input = NodeUtilities.GetInputVariable(nameof(In), context, this);

            int count = GetComponentCount();
            string[] axisNames = { "x", "y", "z", "w" };
            string[] portNames = { nameof(X), nameof(Y), nameof(Z), nameof(W) };

            for (int i = 0; i < count; i++)
            {
                if (!IsPortConnected(portNames[i])) continue;

                string variableName = $"{portNames[i].ToLowerInvariant()}_{GUIDParse.GetGUIDParse(GUID)}";

                Argument argument = new(input.Type, $"{input.Name}.{axisNames[i]}");
                ExpressionRule rule = new("#", typeof(float),
                    ArgumentConstraint.OfCategory(0, ArgumentCategory.Vector));

                Expression expression = new(argument, rule);

                DeclarationStatement statement = new(typeof(float), variableName, expression);

                context.InvokeStatements.Add(statement);
                context.OutputLocals[PortKey.Of(GUID, portNames[i])] = (typeof(float), variableName);
            }
        }

        private bool IsPortConnected(string portName)
        {
            NodePort port = outputPorts.FirstOrDefault(p => p.portData.identifier == portName);
            int edgeCount = port?.GetEdges().Count ?? -1;
            Debug.Log($"portName={portName}, found={port != null}, edgeCount={edgeCount}");
            return port != null && edgeCount > 0;
        }

        private int GetComponentCount()
        {
            if (CurrentType == typeof(Vector2Port)) return ComponentCountY;
            if (CurrentType == typeof(Vector3Port)) return ComponentCountZ;
            if (CurrentType == typeof(Vector4Port)) return ComponentCountW;

            return ComponentCountX;
        }

        public override IEnumerable<FieldInfo> OverrideFieldOrder(IEnumerable<FieldInfo> fields)
        {
            string[] order = { nameof(In), nameof(X), nameof(Y), nameof(Z), nameof(W) };
            return fields.OrderBy(f => Array.IndexOf(order, f.Name));
        }

        [CustomPortBehavior(nameof(X))]
        private IEnumerable<PortData> XPortBehavior(List<SerializableEdge> edges)
        {
            int count = GetComponentCount();

            if (count >= ComponentCountX)
                yield return new PortData { identifier = nameof(X), displayName = nameof(X), displayType = typeof(NumberPort), acceptMultipleEdges = true };
            if (count >= ComponentCountY)
                yield return new PortData { identifier = nameof(Y), displayName = nameof(Y), displayType = typeof(NumberPort), acceptMultipleEdges = true };
            if (count >= ComponentCountZ)
                yield return new PortData { identifier = nameof(Z), displayName = nameof(Z), displayType = typeof(NumberPort), acceptMultipleEdges = true };
            if (count >= ComponentCountW)
                yield return new PortData { identifier = nameof(W), displayName = nameof(W), displayType = typeof(NumberPort), acceptMultipleEdges = true };
        }

        [CustomPortBehavior(nameof(Y))]
        private IEnumerable<PortData> YPortBehaviorNoop(List<SerializableEdge> edges)
        {
            yield break;
        }

        [CustomPortBehavior(nameof(Z))]
        private IEnumerable<PortData> ZPortBehaviorNoop(List<SerializableEdge> edges)
        {
            yield break;
        }

        [CustomPortBehavior(nameof(W))]
        private IEnumerable<PortData> WPortBehaviorNoop(List<SerializableEdge> edges)
        {
            yield break;
        }
    }
}
