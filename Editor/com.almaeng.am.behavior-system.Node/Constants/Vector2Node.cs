using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;
using UnityEngine;

namespace AMBehaviorSystem.Node.Constants
{
    [Serializable]
    [NodeMenuItem("Constant/Vector2")]
    public class Vector2Node : BaseValueNode<Vector2, Vector2Port>, IConstantNode
    {
        public override string name => "Vector2";
    }
}
