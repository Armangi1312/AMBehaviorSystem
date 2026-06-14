using AMBehaviorSystem.Node.Primitives;
using GraphProcessor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AMBehaviorSystem.Editor.Node.Primitives
{
    [NodeCustomEditor(typeof(Vector2Node))]
    public class Vector2NodeView : PrimitiveNodeView<Vector2Node, Vector2Field, Vector2> { }
}
