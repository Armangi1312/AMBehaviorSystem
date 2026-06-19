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
        public override string name => "Distance";

        [Input] public Port A;
        [Input] public Port B;

        [CustomPortBehavior(nameof(A))]
        public IEnumerable<PortData> APort(List<SerializableEdge> _) => CreatePort("A", false);

        [CustomPortBehavior(nameof(B))]
        public IEnumerable<PortData> BPort(List<SerializableEdge> _) => CreatePort("B", false);
    }

    [Serializable]
    [NodeMenuItem("Math/Advanced/Dot")]
    public class DotNode : BaseChangeableMathNode<NumberPort>
    {
        public override string name => "Dot";

        [Input] public Port A;
        [Input] public Port B;

        [CustomPortBehavior(nameof(A))]
        public IEnumerable<PortData> APort(List<SerializableEdge> _) => CreatePort("A", false);

        [CustomPortBehavior(nameof(B))]
        public IEnumerable<PortData> BPort(List<SerializableEdge> _) => CreatePort("B", false);
    }

    [Serializable]
    [NodeMenuItem("Math/Advanced/Magnitude")]
    public class MagnitudeNode : BaseChangeableMathNode<NumberPort>
    {
        public override string name => "Magnitude";

        [Input] public Port In;

        [CustomPortBehavior(nameof(In))]
        public IEnumerable<PortData> InPort(List<SerializableEdge> _) => CreatePort("In", false);
    }

    [Serializable]
    [NodeMenuItem("Math/Advanced/Normalize")]
    public class NormalizeNode : BaseChangeableMathNode<NumberPort>
    {
        public override string name => "Normalize";

        [Input] public Port In;

        [CustomPortBehavior(nameof(In))]
        public IEnumerable<PortData> InPort(List<SerializableEdge> _) => CreatePort("In", false);
    }

    [Serializable]
    [NodeMenuItem("Math/Advanced/Lerp")]
    public class LerpNode : BaseChangeableMathNode
    {
        public override string name => "Lerp";

        [Input] public Port A;
        [Input] public Port B;
        [Input] public NumberPort T;

        [CustomPortBehavior(nameof(A))]
        public IEnumerable<PortData> APort(List<SerializableEdge> _) => CreatePort("A", false);

        [CustomPortBehavior(nameof(B))]
        public IEnumerable<PortData> BPort(List<SerializableEdge> _) => CreatePort("B", false);
    }

    [Serializable]
    [NodeMenuItem("Math/Advanced/Split")]
    public class SplitNode : BaseSplitableMathNode
    {
        public override string name => "Split";

        [Input] public VectorPort In;

        [CustomPortBehavior(nameof(In))]
        public IEnumerable<PortData> InPort(List<SerializableEdge> _) => CreatePort(nameof(In), true);

        [Output] public NumberPort X;
        [Output] public NumberPort Y;
        [Output] public NumberPort Z;
        [Output] public NumberPort W;
    }
}
