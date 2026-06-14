using AMBehaviorSystem.Node.Primitives;
using GraphProcessor;
using UnityEngine.UIElements;

namespace AMBehaviorSystem.Editor.Node.Primitives
{
    [NodeCustomEditor(typeof(IntegerNode))]
    public class IntegerNodeView : PrimitiveNodeView<IntegerNode, IntegerField, int> { }
}
