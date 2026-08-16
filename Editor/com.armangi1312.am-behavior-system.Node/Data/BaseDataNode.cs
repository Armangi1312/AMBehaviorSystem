using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Data
{
    [Serializable]
    public abstract class BaseDataNode : BaseNode, IDataNode
    {
        public string TypeName;
        public string Path;
        public string OutputTypeName;

        public Type Type => string.IsNullOrEmpty(TypeName) ? null : Type.GetType(TypeName);
        public Type OutputType => string.IsNullOrEmpty(OutputTypeName) ? null : Type.GetType(OutputTypeName);

        [Output, NonSerialized] public object Out;
    }
}
