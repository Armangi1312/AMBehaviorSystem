using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Math
{
    [Serializable]
    public abstract class BaseMathNode<TOut> : BaseNode
        where TOut : Port
    {
        [Output] public TOut Out;
    }
}
