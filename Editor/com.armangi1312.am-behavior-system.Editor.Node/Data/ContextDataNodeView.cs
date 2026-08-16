using AMBehaviorSystem.Node.Data;
using GraphProcessor;

namespace AMBehaviorSystem.Editor.Node.Data
{
    [NodeCustomEditor(typeof(ContextDataNode))]
    internal class ContextDataNodeView : BaseControllerDataNodeView<ContextDataNode, IContext>
    {
        protected override string RowLabel => "Context";
        protected override string UndoTypeLabel => "Updated Node Context TypeName";
        protected override string NoneFoundMessage => "(No IContext found on TargetController)";
        protected override string SelectFirstMessage => "(Select a Context type first)";
        protected override string NotFoundMessage => "(Context not found)";
    }
}