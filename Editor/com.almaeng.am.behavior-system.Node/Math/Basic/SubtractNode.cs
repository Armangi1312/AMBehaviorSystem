using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;
using UnityEngine;

namespace AMBehaviorSystem.Node.Math.Basic
{
    [Serializable]
    [NodeMenuItem("Math/Basic/Subtract")]
    public class SubtractNode : BaseNode, IMathNode
    {
        public override string name => "Subtract";

        [Input] public Port A;
        [Input] public Port B;

        [Output] public Port Out;

        protected override void Enable()
        {
            Debug.Log("Enable");
        }
    }
}
