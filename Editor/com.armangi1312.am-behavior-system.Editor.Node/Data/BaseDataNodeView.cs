using AMBehaviorSystem.Editor.Node;
using AMBehaviorSystem.Editor.Utilities;
using AMBehaviorSystem.Node.Data;
using GraphProcessor;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AMBehaviorSystem.Editor.Node.Data
{
    internal abstract class BaseDataNodeView<TNode> : BaseNodeView where TNode : BaseDataNode
    {
        protected Label PathLabel { get; private set; }
        protected TNode Node { get; private set; }

        private readonly Dictionary<string, Type> pathTypeMap = new();

        public override void Enable()
        {
            Node = nodeTarget as TNode;
            PathLabel = NodeViewUIHelper.BuildEllipsisLabel(FormatPathText());

            Button pathButton = new(OnClickPathButton) { text = "▾", style = { width = 24 } };
            controlsContainer.Add(NodeViewUIHelper.BuildRow("Path", PathLabel, pathButton));

            OnEnabled();
        }

        protected virtual void OnEnabled() { }
        protected virtual void OnSelected(string path) { }

        protected abstract List<PathEntry> BuildPathOptions();
        protected abstract bool CanSelectPath();
        protected abstract string CannotSelectPathMessage { get; }

        protected void UpdatePathLabel()
        {
            if(PathLabel == null)
                return;
            PathLabel.text = FormatPathText();
        }

        private string FormatPathText() => string.IsNullOrEmpty(Node.Path) ? "(none)" : Node.Path;

        private void OnClickPathButton()
        {
            if(!CanSelectPath())
            {
                NodeViewUIHelper.ShowDisabledMenu(CannotSelectPathMessage);
                return;
            }

            List<PathEntry> entries = BuildPathOptions();

            pathTypeMap.Clear();
            List<string> paths = new(entries.Count);
            foreach(PathEntry entry in entries)
            {
                paths.Add(entry.Path);
                pathTypeMap[entry.Path] = entry.Type;
            }

            NodeViewUIHelper.ShowPathMenu(paths, Node.Path, path =>
            {
                owner.RegisterCompleteObjectUndo("Updated Node Path");
                Node.Path = path;
                Node.OutputTypeName = pathTypeMap.TryGetValue(path, out Type type) ? type.AssemblyQualifiedName : string.Empty;
                OnSelected(path);
                UpdatePathLabel();
            });
        }
    }
}