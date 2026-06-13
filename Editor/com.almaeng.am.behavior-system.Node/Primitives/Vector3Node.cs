using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;
using UnityEngine;

namespace AMBehaviorSystem.Node.Primitives
{
    [Serializable]
    [NodeMenuItem("Primitives/Vector3")]
    public class Vector3Node : BasePrimitiveNode<Vector3, Vector3Port> 
    {
        public override string name => "Vector3";
    }
}
