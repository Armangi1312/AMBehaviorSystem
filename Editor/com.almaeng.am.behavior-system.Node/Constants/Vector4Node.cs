using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;
using UnityEngine;

namespace AMBehaviorSystem.Node.Constants
{
    [Serializable]
    [NodeMenuItem("Constant/Vector4")]
    public class Vector4Node : BaseValueNode<Vector4, Vector4Port>, IConstantNode
    {
        public override string name => "Vector4";
    }
}
