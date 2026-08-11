using AMBehaviorSystem.Node.Ports;
using AMBehaviorSystem.Node.Constants;
using GraphProcessor;
using UnityEngine.UIElements;

namespace AMBehaviorSystem.Editor.Node.Constants
{
    [NodeCustomEditor(typeof(IntegerNode))]
    public class IntegerNodeView : BaseValueNodeView<IntegerNode, IntegerField, int, NumberPort> { }
}
