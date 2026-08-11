using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Data
{
    [Serializable]
    public abstract class BaseDataNode : BaseNode, IDataNode
    {
        public string Type;
        public string Path;

        [Output, NonSerialized] public object Out;
    }
}
