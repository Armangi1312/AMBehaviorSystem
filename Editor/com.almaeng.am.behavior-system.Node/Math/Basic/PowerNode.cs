using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;
using System.Collections.Generic;

namespace AMBehaviorSystem.Node.Math.Basic
{
    [Serializable]
    [NodeMenuItem("Math/Basic/Power")]
    public class PowerNode : BaseChangeableMathNode
    {
        [Input] public Port A;

        [Input] public NumberPort B;

        public override string name => "Power";

        [CustomPortBehavior(nameof(A))]
        public IEnumerable<PortData> APort(List<SerializableEdge> _) => CreatePort("A", false);
    }
}