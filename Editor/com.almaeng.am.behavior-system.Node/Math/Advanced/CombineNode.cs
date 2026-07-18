using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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

        private Type ResolveOutputType()
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
            yield return new PortData { identifier = nameof(Out), displayName = nameof(Out), displayType = ResolveOutputType() };
        }
    }
}