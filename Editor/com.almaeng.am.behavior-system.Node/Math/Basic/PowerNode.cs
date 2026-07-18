using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;
using UnityEngine;

namespace AMBehaviorSystem.Node.Math.Basic
{
    [Serializable]
    [NodeMenuItem("Math/Basic/Power")]
    public class PowerNode : BaseNode, IMathNode
    {
        public override string name => "Power";

        [Input] public NumberPort A;
        [Input] public NumberPort B;

        [Output] public NumberPort Out;
    }
}
