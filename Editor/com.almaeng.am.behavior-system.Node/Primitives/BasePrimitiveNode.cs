using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Primitives
{
    [Serializable]
    public abstract class BasePrimitiveNode<TField> : BaseNode
        where TField : struct
    {
        public TField Field;
    }

    [Serializable]
    public abstract class BasePrimitiveNode<TField, TOutPort> : BasePrimitiveNode<TField>
        where TField : struct
        where TOutPort : Port
    {
        [Output] public TOutPort Out;
    }
}