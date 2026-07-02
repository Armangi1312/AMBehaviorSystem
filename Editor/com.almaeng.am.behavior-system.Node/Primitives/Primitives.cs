using AMBehaviorSystem.Node.Math;
using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;
using UnityEngine;

namespace AMBehaviorSystem.Node.Primitives
{
    [Serializable]
    public abstract class BasePrimitiveNode<TField, TOutput> : BaseNode, IPrimitiveNode, IOutNode<TOutput>, IFieldNode<TField>
        where TField : struct
        where TOutput : Port
    {
        [field: Output, SerializeField] public TOutput Out { get; set; }
        [field: SerializeField] public TField Field { get; set; }
    }

    [Serializable]
    [NodeMenuItem("Primitives/Boolean")]
    public class BooleanNode : BasePrimitiveNode<bool, BooleanPort>
    {
        public override string name => "Boolean";
    }

    [Serializable]
    [NodeMenuItem("Primitives/Integer")]
    public class IntegerNode : BasePrimitiveNode<int, NumberPort>
    {
        public override string name => "Integer";
    }

    [Serializable]
    [NodeMenuItem("Primitives/Float")]
    public class FloatNode : BasePrimitiveNode<float, NumberPort>
    {
        public override string name => "Float";
    }

    [Serializable]
    [NodeMenuItem("Primitives/Double")]
    public class DoubleNode : BasePrimitiveNode<double, NumberPort>
    {
        public override string name => "Double";
    }

    [Serializable]
    [NodeMenuItem("Primitives/Vector2")]
    public class Vector2Node : BasePrimitiveNode<Vector2, Vector2Port>
    {
        public override string name => "Vector2";
    }

    [Serializable]
    [NodeMenuItem("Primitives/Vector3")]
    public class Vector3Node : BasePrimitiveNode<Vector3, Vector3Port>
    {
        public override string name => "Vector3";
    }

    [Serializable]
    [NodeMenuItem("Primitives/Vector4")]
    public class Vector4Node : BasePrimitiveNode<Vector4, Vector4Port>
    {
        public override string name => "Vector4";
    }
}
