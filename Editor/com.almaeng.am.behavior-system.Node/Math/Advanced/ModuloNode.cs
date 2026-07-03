using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Math.Advanced
{
    [Serializable]
    [NodeMenuItem("Math/Advanced/Modulo")]
    public class ModuloNode : BaseNode, IMathNode
    {
        public override string name => "Modulo";

        [Input] public NumberPort A;
        [Input] public NumberPort B;

        [Output] public NumberPort Out;
    }
}
