using AMBehaviorSystem.Node.Ports;
using AMBehaviorSystem.Node.Constants;
using GraphProcessor;
using UnityEngine.UIElements;

namespace AMBehaviorSystem.Editor.Node.Constants
{
    [NodeCustomEditor(typeof(BooleanNode))]
    public class BooleanNodeView : BaseValueNodeView<BooleanNode, Toggle, bool, BooleanPort> { }
}
