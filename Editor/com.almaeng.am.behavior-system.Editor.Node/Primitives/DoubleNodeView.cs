using AMBehaviorSystem.Node.Primitives;
using GraphProcessor;
using UnityEngine.UIElements;

namespace AMBehaviorSystem.Editor.Node.Primitives
{
    [NodeCustomEditor(typeof(DoubleNode))]
    public class DoubleNodeView : PrimitiveNodeView<DoubleNode, DoubleField, double> { }
}
