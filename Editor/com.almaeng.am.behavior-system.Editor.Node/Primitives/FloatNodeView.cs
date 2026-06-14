using AMBehaviorSystem.Node.Primitives;
using GraphProcessor;
using UnityEngine.UIElements;

namespace AMBehaviorSystem.Editor.Node.Primitives
{
    [NodeCustomEditor(typeof(FloatNode))]
    public class FloatNodeView : PrimitiveNodeView<FloatNode, FloatField, float> { }
}
