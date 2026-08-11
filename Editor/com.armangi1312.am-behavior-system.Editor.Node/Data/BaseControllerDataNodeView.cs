using AMBehaviorSystem.Node;
using AMBehaviorSystem.Node.Data;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AMBehaviorSystem.Editor.Node.Data
{
    public abstract class BaseControllerDataNodeView<TNode, TInterface> : BaseDataNodeView<TNode>
        where TNode : BaseDataNode
        where TInterface : class
    {
        private Label typeLabel;

        protected abstract string RowLabel { get; }
        protected abstract string UndoTypeLabel { get; }
        protected abstract string NoneFoundMessage { get; }
        protected abstract string SelectFirstMessage { get; }
        protected abstract string NotFoundMessage { get; }

        protected override string CannotSelectPathMessage => SelectFirstMessage;

        protected override void OnEnabled()
        {
            typeLabel = NodeViewUIHelper.BuildEllipsisLabel(string.IsNullOrEmpty(Node.Type) ? "(none)" : Node.Type);
            Button typeButton = new(OnClickTypeButton) { text = "▾", style = { width = 24 } };
            controlsContainer.Insert(0, NodeViewUIHelper.BuildRow(RowLabel, typeLabel, typeButton));
        }

        protected override bool CanSelectPath() => !string.IsNullOrEmpty(Node.Type);

        protected override List<string> BuildPathOptions()
        {
            TInterface target = FindItemByTypeName(Node.Type);
            if (target == null) return new List<string>();

            List<string> paths = new();
            ReflectionPathUtility.CollectPaths(target.GetType(), string.Empty, paths, 0);
            return paths;
        }

        private void OnClickTypeButton()
        {
            List<TInterface> items = GetItemsFromGraph();
            if (items == null || items.Count == 0)
            {
                NodeViewUIHelper.ShowDisabledMenu(NoneFoundMessage);
                return;
            }

            GenericMenu menu = new();
            foreach (TInterface item in items)
            {
                if (item == null) continue;
                string typeName = item.GetType().Name;
                menu.AddItem(new GUIContent(typeName), typeName == Node.Type, () => SelectType(typeName));
            }
            menu.ShowAsContext();
        }

        private void SelectType(string typeName)
        {
            owner.RegisterCompleteObjectUndo(UndoTypeLabel);
            Node.Type = typeName;
            Node.Path = string.Empty;
            typeLabel.text = typeName;
            UpdatePathLabel();
        }

        private List<TInterface> GetItemsFromGraph()
        {
            if (owner.graph is not NodeGraph nodeGraph) return null;
            nodeGraph.Resolve();
            return ControllerRegistryScanner.ExtractItems<TInterface>(nodeGraph.TargetController);
        }

        private TInterface FindItemByTypeName(string typeName)
        {
            List<TInterface> items = GetItemsFromGraph();
            if (items == null) return null;
            foreach (TInterface item in items)
                if (item?.GetType().Name == typeName) return item;
            return null;
        }
    }
}