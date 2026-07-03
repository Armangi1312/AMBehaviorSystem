using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
                yield return new PortData { identifier = nameof(X), displayName = nameof(X), displayType = typeof(NumberPort) };
            if (count >= ComponentCountY)
                yield return new PortData { identifier = nameof(Y), displayName = nameof(Y), displayType = typeof(NumberPort) };
            if (count >= ComponentCountZ)
                yield return new PortData { identifier = nameof(Z), displayName = nameof(Z), displayType = typeof(NumberPort) };
            if (count >= ComponentCountW)
                yield return new PortData { identifier = nameof(W), displayName = nameof(W), displayType = typeof(NumberPort) };
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
