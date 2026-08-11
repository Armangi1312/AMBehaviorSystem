using AMBehaviorSystem.Node.Ports;
using AMBehaviorSystem.Node.Constants;
using GraphProcessor;
using UnityEngine.UIElements;

namespace AMBehaviorSystem.Editor.Node.Constants
{
    [NodeCustomEditor(typeof(FloatNode))]
    public class FloatNodeView : BaseValueNodeView<FloatNode, FloatField, float, NumberPort> { }
}
