using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;
using UnityEngine;

namespace AMBehaviorSystem.Node.Math.Advanced
{
    [Serializable]
    [NodeMenuItem("Math/Advanced/Random")]
    public class RandomNode : BaseMathNode<NumberPort>
    {
        [Input] public NumberPort Min;

        [Input] public NumberPort Man;

        public NumberType OutputType;

        public override string name => "Atan2";
    }
}
