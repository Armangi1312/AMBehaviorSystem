using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;
using System.Collections.Generic;

namespace AMBehaviorSystem.Node.Math.Advanced
{
    [Serializable]
    [NodeMenuItem("Math/Advanced/Lerp")]
    public class LerpNode : BaseChangeableMathNode
    {
        [Input] public Port A;

        [Input] public Port B;

        [Input] public NumberPort T;

        public override string name => "Add";

        [CustomPortBehavior(nameof(A))]
        public IEnumerable<PortData> APort(List<SerializableEdge> _) => CreatePort("A", false);

        [CustomPortBehavior(nameof(B))]
        public IEnumerable<PortData> BPort(List<SerializableEdge> _) => CreatePort("B", false);
    }
}