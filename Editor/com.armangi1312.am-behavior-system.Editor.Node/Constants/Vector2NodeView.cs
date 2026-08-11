using AMBehaviorSystem.Node.Ports;
using AMBehaviorSystem.Node.Constants;
using GraphProcessor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AMBehaviorSystem.Editor.Node.Constants
{
    [NodeCustomEditor(typeof(Vector2Node))]
    public class Vector2NodeView : BaseValueNodeView<Vector2Node, Vector2Field, Vector2, Vector2Port> { }
}
