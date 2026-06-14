using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;
using System.Collections.Generic;

namespace AMBehaviorSystem.Node.Math.Advanced
{
    [Serializable]
    [NodeMenuItem("Math/Advanced/Magnitude")]
    public class MagnitudeNode : BaseChangeableMathNode<NumberPort>
    {
        [Input] public Port In;
        public override string name => "Magnitude";

        [CustomPortBehavior(nameof(In))]
        public IEnumerable<PortData> InPort(List<SerializableEdge> _) => CreatePort("In", false);
    }
}