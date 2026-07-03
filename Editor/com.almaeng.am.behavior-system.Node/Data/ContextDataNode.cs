using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Data
{
    [Serializable]
    [NodeMenuItem("Data/Context Data")]
    public class ContextDataNode : BaseDataNode
    {
        public override string name => "Context Data";
    }
}
