using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Data
{
    [Serializable]
    public abstract class BaseDataNode<TOut> : BaseNode 
    {
        [Output] public TOut Out;
    }
}
