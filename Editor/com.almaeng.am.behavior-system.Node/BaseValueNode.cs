using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node
{
    [Serializable]
    public abstract class BaseValueNode<TField, TOutput> : BaseNode
        where TField : struct
        where TOutput : Port
    {
        [Output] public TOutput Out;
        public TField Field;
    }
}
