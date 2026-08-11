using AMBehaviorSystem.Node.Ports;
using AMBehaviorSystem.Node.Constants;
using GraphProcessor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AMBehaviorSystem.Editor.Node.Constants
{
    [NodeCustomEditor(typeof(Vector4Node))]
    public class Vector4NodeView : BaseValueNodeView<Vector4Node, Vector4Field, Vector4, Vector4Port> { }
}
