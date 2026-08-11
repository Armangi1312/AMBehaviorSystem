using AMBehaviorSystem.Node.Ports;
using AMBehaviorSystem.Node.Constants;
using GraphProcessor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AMBehaviorSystem.Editor.Node.Constants
{
    [NodeCustomEditor(typeof(Vector3Node))]
    public class Vector3NodeView : BaseValueNodeView<Vector3Node, Vector3Field, Vector3, Vector3Port> { }
}
