using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;
using UnityEngine;

namespace AMBehaviorSystem.Node.Math.Basic
{
    [Serializable]
    [NodeMenuItem("Math/Basic/Add")]
    public class AddNode : BaseDynamicTypeNode, IMathNode
    {
        public override string name => "Add";

        protected override Type DefaultType => typeof(NumberPort);

        [Input] public Port A;
        [Input] public Port B;

        [Output] public Port Out;
    }
}
