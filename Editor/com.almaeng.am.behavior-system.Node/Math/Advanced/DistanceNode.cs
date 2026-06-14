using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;
using System.Collections.Generic;

namespace AMBehaviorSystem.Node.Math.Advanced
{
    [Serializable]
    [NodeMenuItem("Math/Advanced/Distance")]
    public class DistanceNode : BaseChangeableMathNode<NumberPort>
    {
        [Input] public Port A;

        [Input] public Port B;

        public override string name => "Distance";

        [CustomPortBehavior(nameof(A))]
        public IEnumerable<PortData> APort(List<SerializableEdge> _) => CreatePort("A", false);

        [CustomPortBehavior(nameof(B))]
        public IEnumerable<PortData> BPort(List<SerializableEdge> _) => CreatePort("B", false);
    }
}