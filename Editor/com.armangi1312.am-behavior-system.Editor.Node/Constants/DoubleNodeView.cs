using AMBehaviorSystem.Node.Ports;
using AMBehaviorSystem.Node.Constants;
using GraphProcessor;
using UnityEngine.UIElements;

namespace AMBehaviorSystem.Editor.Node.Constants
{
    [NodeCustomEditor(typeof(DoubleNode))]
    public class DoubleNodeView : BaseValueNodeView<DoubleNode, DoubleField, double, NumberPort> { }
}
