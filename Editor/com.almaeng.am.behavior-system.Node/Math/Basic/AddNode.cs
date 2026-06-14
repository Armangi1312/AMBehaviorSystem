using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;
using System.Collections.Generic;

namespace AMBehaviorSystem.Node.Math.Basic
{
    [Serializable]
    [NodeMenuItem("Math/Basic/Add")]
    public class AddNode : BaseChangeableMathNode
    {
        [Input] public Port A;

        [Input] public Port B;

        public override string name => "Add";

        [CustomPortBehavior(nameof(A))]
        public IEnumerable<PortData> APort(List<SerializableEdge> _) => CreatePort("A", false);

        [CustomPortBehavior(nameof(B))]
        public IEnumerable<PortData> BPort(List<SerializableEdge> _) => CreatePort("B", false);
    }
}