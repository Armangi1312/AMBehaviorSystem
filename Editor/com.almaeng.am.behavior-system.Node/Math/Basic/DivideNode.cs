using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;
using UnityEngine;

namespace AMBehaviorSystem.Node.Math.Basic
{
    [Serializable]
    [NodeMenuItem("Math/Basic/Divide")]
    public class DivideNode : BaseDynamicTypeNode, IMathNode
    {
        public override string name => "Divide";

        protected override Type DefaultType => typeof(NumberPort);

        [Input] public Port A;
        [Input] public NumberPort B;

        [Output] public Port Out;
    }
}