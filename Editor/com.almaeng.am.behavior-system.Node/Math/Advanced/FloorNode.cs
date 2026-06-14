using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;
using UnityEngine;

namespace AMBehaviorSystem.Node.Math.Advanced
{
    [Serializable]
    [NodeMenuItem("Math/Advanced/Floor")]
    public class FloorNode : BaseMathNode<NumberPort>
    {
        [Input] public NumberPort In;

        public override string name => "Floor";
    }
}