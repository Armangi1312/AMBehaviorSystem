using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Math.Advanced
{
    [Serializable]
    [NodeMenuItem("Math/Advanced/Modulo")]
    public class ModuloNode : BaseMathNode<NumberPort>
    {
        [Input] public NumberPort A;

        [Input] public NumberPort B;

        public override string name => "Modulo";
    }
}