using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;
using UnityEngine;

namespace AMBehaviorSystem.Node.Primitives
{
    [Serializable]
    [NodeMenuItem("Primitives/Vector2")]
    public class Vector2Node : BasePrimitiveNode<Vector2, Vector2Port> 
    {
        public override string name => "Vector2";
    }
}
