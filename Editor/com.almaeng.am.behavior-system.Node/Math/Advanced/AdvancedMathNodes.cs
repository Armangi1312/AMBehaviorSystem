using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Math.Advanced
{
    [Serializable]
    [NodeMenuItem("Math/Advanced/Abs")]
    public class AbsNode : BaseMathNode<NumberPort>
    {
        public override string name => "Abs";

        [Input] public NumberPort A;
    }

    [Serializable]
    [NodeMenuItem("Math/Advanced/Sin")]
    public class SinNode : BaseMathNode<NumberPort>
    {
        public override string name => "Sin";

        [Input] public NumberPort In;
    }

    [Serializable]
    [NodeMenuItem("Math/Advanced/Cos")]
    public class CosNode : BaseMathNode<NumberPort>
    {
        public override string name => "Cos";

        [Input] public NumberPort In;
    }

    [Serializable]
    [NodeMenuItem("Math/Advanced/Tan")]
    public class TanNode : BaseMathNode<NumberPort>
    {
        public override string name => "Tan";

        [Input] public NumberPort In;
    }

    [Serializable]
    [NodeMenuItem("Math/Advanced/Ceil")]
    public class CeilNode : BaseMathNode<NumberPort>
    {
        public override string name => "Ceil";

        [Input] public NumberPort In;
    }

    [Serializable]
    [NodeMenuItem("Math/Advanced/Floor")]
    public class FloorNode : BaseMathNode<NumberPort>
    {
        public override string name => "Floor";

        [Input] public NumberPort In;
    }

    [Serializable]
    [NodeMenuItem("Math/Advanced/Round")]
    public class RoundNode : BaseMathNode<NumberPort>
    {
        public override string name => "Round";

        [Input] public NumberPort In;
    }

    [Serializable]
    [NodeMenuItem("Math/Advanced/Atan2")]
    public class Atan2Node : BaseMathNode<NumberPort>
    {
        public override string name => "Atan2";

        [Input] public NumberPort Y;
        [Input] public NumberPort X;
    }

    [Serializable]
    [NodeMenuItem("Math/Advanced/Min")]
    public class MinNode : BaseMathNode<NumberPort>
    {
        public override string name => "Min";

        [Input] public NumberPort A;
        [Input] public NumberPort B;
    }

    [Serializable]
    [NodeMenuItem("Math/Advanced/Max")]
    public class MaxNode : BaseMathNode<NumberPort>
    {
        public override string name => "Max";

        [Input] public NumberPort A;
        [Input] public NumberPort B;
    }

    [Serializable]
    [NodeMenuItem("Math/Advanced/Modulo")]
    public class ModuloNode : BaseMathNode<NumberPort>
    {
        public override string name => "Modulo";

        [Input] public NumberPort A;
        [Input] public NumberPort B;
    }

    [Serializable]
    [NodeMenuItem("Math/Advanced/Clamp")]
    public class ClampNode : BaseMathNode<NumberPort>
    {
        public override string name => "Clamp";

        [Input] public NumberPort In;
        [Input] public NumberPort Min;
        [Input] public NumberPort Max;
    }

    [Serializable]
    [NodeMenuItem("Math/Advanced/Remap")]
    public class RemapNode : BaseMathNode<NumberPort>
    {
        public override string name => "Remap";

        [Input] public NumberPort Value;
        [Input] public NumberPort FromMin;
        [Input] public NumberPort FromMax;
        [Input] public NumberPort ToMin;
        [Input] public NumberPort ToMax;
    }

    [Serializable]
    [NodeMenuItem("Math/Advanced/Random")]
    public class RandomNode : BaseMathNode<NumberPort>
    {
        public override string name => "Random";

        [Input] public NumberPort Min;
        [Input] public NumberPort Max;

        public NumberType OutputType;
    }
}
