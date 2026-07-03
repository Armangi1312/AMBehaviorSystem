using AMBehaviorSystem.Node.Base;
using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;
using UnityEngine;

namespace AMBehaviorSystem.Node.Constants
{
    [Serializable]
    [NodeMenuItem("Constant/Vector3")]
    public class Vector3Node : BaseValueNode<Vector3, Vector3Port>, IConstantNode
    {
        public override string name => "Vector3";
    }
}
