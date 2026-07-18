using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;
using UnityEngine;

namespace AMBehaviorSystem.Node.Math.Basic
{
    [Serializable]
    [NodeMenuItem("Math/Basic/Square Root")]
    public class SquareRootNode : BaseNode, IMathNode
    {
        public override string name => "Square Root";

        [Input] public NumberPort In;

        [Output] public NumberPort Out;
    }
}
