using AMBehaviorSystem.Core;
using AMBehaviorSystem.Node.Data;
using GraphProcessor;

namespace AMBehaviorSystem.Editor.Node.Data
{
    [NodeCustomEditor(typeof(SettingDataNode))]
    public class SettingDataNodeView : BaseControllerDataNodeView<SettingDataNode, ISetting>
    {
        protected override string RowLabel => "Setting";
        protected override string UndoTypeLabel => "Updated Node Setting Type";
        protected override string NoneFoundMessage => "(No ISetting found on TargetController)";
        protected override string SelectFirstMessage => "(Select a Setting type first)";
        protected override string NotFoundMessage => "(Setting not found)";
    }
}