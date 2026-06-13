using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;
using UnityEngine;

namespace AMBehaviorSystem.Node.Primitives
{
    [Serializable]
    [NodeMenuItem("Primitives/Vector4")]
    public class Vector4Node : BasePrimitiveNode<Vector4, Vector4Port> 
    {
        public override string name => "Vector4";
    }
}
