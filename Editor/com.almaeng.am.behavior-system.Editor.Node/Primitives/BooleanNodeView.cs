using AMBehaviorSystem.Node.Primitives;
using GraphProcessor;
using UnityEngine.UIElements;

namespace AMBehaviorSystem.Editor.Node.Primitives
{
    [NodeCustomEditor(typeof(BooleanNode))]
    public class BooleanNodeView : PrimitiveNodeView<BooleanNode, Toggle, bool> { }
}
