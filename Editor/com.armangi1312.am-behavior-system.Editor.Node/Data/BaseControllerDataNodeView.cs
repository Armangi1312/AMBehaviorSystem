using AMBehaviorSystem.Editor.Node;
using AMBehaviorSystem.Editor.Utilities;
using AMBehaviorSystem.Node;
using AMBehaviorSystem.Node.Data;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AMBehaviorSystem.Editor.Node.Data
{
    internal abstract class BaseControllerDataNodeView<TNode, TInterface> : BaseDataNodeView<TNode>
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
            string currentTypeName = Node.Type?.Name;
            typeLabel = NodeViewUIHelper.BuildEllipsisLabel(string.IsNullOrEmpty(currentTypeName) ? "(none)" : currentTypeName);
            Button typeButton = new(OnClickTypeButton) { text = "▾", style = { width = 24 } };
            controlsContainer.Insert(0, NodeViewUIHelper.BuildRow(RowLabel, typeLabel, typeButton));
        }

        protected override bool CanSelectPath() => !string.IsNullOrEmpty(Node.TypeName);

        protected override List<PathEntry> BuildPathOptions()
        {
            TInterface target = FindItemByAssemblyQualifiedName(Node.TypeName);
            if(target == null)
                return new List<PathEntry>();

            List<PathEntry> entries = new();
            ReflectionPathUtilities.CollectPaths(target.GetType(), string.Empty, entries, 0);
            return entries;
        }

        private void OnClickTypeButton()
        {
            List<TInterface> items = GetItemsFromGraph();
            if(items == null || items.Count == 0)
            {
                NodeViewUIHelper.ShowDisabledMenu(NoneFoundMessage);
                return;
            }

            GenericMenu menu = new();
            foreach(TInterface item in items)
            {
                if(item == null)
                    continue;
                Type itemType = item.GetType();
                menu.AddItem(new GUIContent(itemType.Name), itemType.AssemblyQualifiedName == Node.TypeName, () => SelectType(itemType));
            }
            menu.ShowAsContext();
        }

        private void SelectType(Type type)
        {
            owner.RegisterCompleteObjectUndo(UndoTypeLabel);
            Node.TypeName = type.AssemblyQualifiedName;
            Node.Path = string.Empty;
            Node.OutputTypeName = string.Empty;
            typeLabel.text = type.Name;
            UpdatePathLabel();
        }

        private List<TInterface> GetItemsFromGraph()
        {
            if(owner.graph is not NodeGraph nodeGraph)
                return null;
            nodeGraph.Resolve();
            return ControllerRegistryScanner.ExtractItems<TInterface>(nodeGraph.TargetController);
        }

        private TInterface FindItemByAssemblyQualifiedName(string typeName)
        {
            List<TInterface> items = GetItemsFromGraph();
            if(items == null)
                return null;
            foreach(TInterface item in items)
                if(item?.GetType().AssemblyQualifiedName == typeName)
                    return item;
            return null;
        }
    }
}