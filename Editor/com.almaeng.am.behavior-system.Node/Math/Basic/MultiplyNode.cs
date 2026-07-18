using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;
using UnityEngine;

namespace AMBehaviorSystem.Node.Math.Basic
{
    [Serializable]
    [NodeMenuItem("Math/Basic/Multiply")]
    public class MultiplyNode : BaseDynamicTypeNode, IMathNode
    {
        public override string name => "Multiply";

        protected override Type DefaultType => typeof(NumberPort);

        [Input] public Port A;
        [Input] public NumberPort B;

        [Output] public Port Out;
    }
}