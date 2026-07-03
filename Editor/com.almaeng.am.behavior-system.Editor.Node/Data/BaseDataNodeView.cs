// Editor/Node/Data/BaseDataNodeView.cs
using AMBehaviorSystem.Node.Data;
using GraphProcessor;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace AMBehaviorSystem.Editor.Node.Data
{
    public abstract class BaseDataNodeView<TNode> : BaseNodeView where TNode : BaseDataNode
    {
        protected Label PathLabel { get; private set; }
        protected TNode Node { get; private set; }

        public override void Enable()
        {
            Node = nodeTarget as TNode;
            PathLabel = NodeViewUIHelper.BuildEllipsisLabel(FormatPathText());

            Button pathButton = new(OnClickPathButton) { text = "▾", style = { width = 24 } };
            controlsContainer.Add(NodeViewUIHelper.BuildRow("Path", PathLabel, pathButton));

            OnEnabled();
        }

        protected virtual void OnEnabled() { }

        protected abstract List<string> BuildPathOptions();
        protected abstract bool CanSelectPath();
        protected abstract string CannotSelectPathMessage { get; }

        protected void UpdatePathLabel()
        {
            if (PathLabel == null) return;
            PathLabel.text = FormatPathText();
        }

        private string FormatPathText() => string.IsNullOrEmpty(Node.Path) ? "(none)" : Node.Path;

        private void OnClickPathButton()
        {
            if (!CanSelectPath())
            {
                NodeViewUIHelper.ShowDisabledMenu(CannotSelectPathMessage);
                return;
            }

            List<string> paths = BuildPathOptions();
            NodeViewUIHelper.ShowPathMenu(paths, Node.Path, path =>
            {
                owner.RegisterCompleteObjectUndo("Updated Node Path");
                Node.Path = path;
                UpdatePathLabel();
            });
        }
    }
}