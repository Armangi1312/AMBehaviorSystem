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
        public override string name => "Add";

        [Input] public NumberPort A;
        [Input] public NumberPort B;

        [CustomPortBehavior(nameof(A))]
        public IEnumerable<PortData> APort(List<SerializableEdge> _) => CreatePort("In", false);

        [CustomPortBehavior(nameof(B))]
        public IEnumerable<PortData> BPort(List<SerializableEdge> _) => CreatePort("B", false);
    }

    [Serializable]
    [NodeMenuItem("Math/Basic/Subtract")]
    public class SubtractNode : BaseChangeableMathNode
    {
        public override string name => "Subtract";

        [Input] public NumberPort A;
        [Input] public NumberPort B;

        [CustomPortBehavior(nameof(A))]
        public IEnumerable<PortData> APort(List<SerializableEdge> _) => CreatePort("In", false);

        [CustomPortBehavior(nameof(B))]
        public IEnumerable<PortData> BPort(List<SerializableEdge> _) => CreatePort("B", false);
    }

    [Serializable]
    [NodeMenuItem("Math/Basic/Multiply")]
    public class MultiplyNode : BaseChangeableMathNode
    {
        public override string name => "Multiply";

        [Input] public NumberPort A;
        [Input] public NumberPort B;

        [CustomPortBehavior(nameof(A))]
        public IEnumerable<PortData> APort(List<SerializableEdge> _) => CreatePort("In", false);
    }

    [Serializable]
    [NodeMenuItem("Math/Basic/Divide")]
    public class DivideNode : BaseChangeableMathNode
    {
        public override string name => "Divide";

        [Input] public NumberPort A;
        [Input] public NumberPort B;

        [CustomPortBehavior(nameof(A))]
        public IEnumerable<PortData> APort(List<SerializableEdge> _) => CreatePort("In", false);
    }

    [Serializable]
    [NodeMenuItem("Math/Basic/Power")]
    public class PowerNode : BaseChangeableMathNode
    {
        public override string name => "Power";

        [Input] public NumberPort A;
        [Input] public NumberPort B;

        [CustomPortBehavior(nameof(A))]
        public IEnumerable<PortData> APort(List<SerializableEdge> _) => CreatePort("In", false);
    }

    [Serializable]
    [NodeMenuItem("Math/Basic/Square Root")]
    public class SquareRootNode : BaseChangeableMathNode
    {
        public override string name => "Square Root";

        [Input] public NumberPort In;
    }
}
