using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;
using System.Collections.Generic;

namespace AMBehaviorSystem.Node.Math.Basic
{
    [Serializable]
    [NodeMenuItem("Math/Basic/Multiply")]
    public class MultiplyNode : BaseChangeableMathNode
    {
        [Input] public Port A;

        [Input] public NumberPort B;

        public override string name => "Multiply";

        [CustomPortBehavior(nameof(A))]
        public IEnumerable<PortData> APort(List<SerializableEdge> _) => CreatePort("A", false);
    }
}