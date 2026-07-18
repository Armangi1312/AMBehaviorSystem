using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;
using UnityEngine;

namespace AMBehaviorSystem.Node.Math.Basic
{
    [Serializable]
    [NodeMenuItem("Math/Basic/Subtract")]
    public class SubtractNode : BaseDynamicTypeNode, IMathNode
    {
        public override string name => "Subtract";

        protected override Type DefaultType => typeof(NumberPort);

        [Input] public Port A;
        [Input] public Port B;

        [Output] public Port Out;
    }
}
