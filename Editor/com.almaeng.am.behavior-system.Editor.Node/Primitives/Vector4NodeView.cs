using AMBehaviorSystem.Node.Primitives;
using GraphProcessor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AMBehaviorSystem.Editor.Node.Primitives
{
    [NodeCustomEditor(typeof(Vector4Node))]
    public class Vector4NodeView : PrimitiveNodeView<Vector4Node, Vector4Field, Vector4> { }
}
