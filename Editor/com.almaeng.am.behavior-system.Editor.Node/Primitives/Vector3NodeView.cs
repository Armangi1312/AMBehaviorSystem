using AMBehaviorSystem.Node.Primitives;
using GraphProcessor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AMBehaviorSystem.Editor.Node.Primitives
{
    [NodeCustomEditor(typeof(Vector3Node))]
    public class Vector3NodeView : PrimitiveNodeView<Vector3Node, Vector3Field, Vector3> { }
}
