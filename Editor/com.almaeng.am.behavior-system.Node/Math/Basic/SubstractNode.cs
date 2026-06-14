using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;
using System.Collections.Generic;

namespace AMBehaviorSystem.Node.Math.Basic
{
    [Serializable]
    [NodeMenuItem("Math/Basic/Substract")]
    public class SubstractNode : BaseChangeableMathNode
    {
        [Input] public Port A;

        [Input] public Port B;

        public override string name => "Substract";

        [CustomPortBehavior(nameof(A))]
        public IEnumerable<PortData> APort(List<SerializableEdge> _) => CreatePort("A", false);

        [CustomPortBehavior(nameof(B))]
        public IEnumerable<PortData> BPort(List<SerializableEdge> _) => CreatePort("B", false);
    }
}