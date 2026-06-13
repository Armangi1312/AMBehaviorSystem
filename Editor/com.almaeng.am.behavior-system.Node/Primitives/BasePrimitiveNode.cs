using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Primitives
{
    [Serializable]
    public abstract class BasePrimitiveNode<TField, TOutPort> : BaseNode
        where TOutPort : Port
    {
        public TField Field;
        [Output] public TOutPort Out;
    }
}
