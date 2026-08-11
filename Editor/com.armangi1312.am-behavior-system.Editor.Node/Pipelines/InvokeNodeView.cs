using AMBehaviorSystem.Node;
using AMBehaviorSystem.Node.Pipelines;
using GraphProcessor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AMBehaviorSystem.Editor.Node.Pipelines
{
    [NodeCustomEditor(typeof(InvokeNode))]
    public class InvokeNodeView : BaseNodeView
    {
        private NodeGraph Graph => owner.graph as NodeGraph;
        private InvokeNode TargetNode => nodeTarget as InvokeNode;

        public override void Enable()
        {
            InvokeNode node = TargetNode;

            ListView listView = new()
            {
                reorderable = false,
                showAddRemoveFooter = true,
                showBorder = true,
                showFoldoutHeader = false,
                itemsSource = node.ProcessorTypes,
                makeItem = () => new Label(),
                bindItem = (element, index) =>
                {
                    Label label = element as Label;
                    label.text = FormatTypeName(node.ProcessorTypes[index]);
                }
            };

            listView.itemsRemoved += _ =>
            {
                owner.RegisterCompleteObjectUndo("Remove Processor");
                listView.Rebuild();
            };

            listView.onAdd = _ => ShowAddMenu(listView, node);

            controlsContainer.Add(listView);
        }

        private void ShowAddMenu(ListView listView, InvokeNode node)
        {
            Object controller = Graph != null ? Graph.TargetController : null;

            if (controller == null) return;

            SerializedObject serializedObject = new(controller);
            SerializedProperty processorsProperty = serializedObject.FindProperty("<Processors>k__BackingField");
            SerializedProperty itemsProperty = processorsProperty?.FindPropertyRelative("Items");

            if (itemsProperty == null) return;

            GenericMenu menu = new();
            HashSet<string> existingTypes = new(node.ProcessorTypes);

            for (int i = 0; i < itemsProperty.arraySize; i++)
            {
                SerializedProperty element = itemsProperty.GetArrayElementAtIndex(i);
                string fullTypeName = element.managedReferenceFullTypename;

                if (string.IsNullOrEmpty(fullTypeName)) continue;

                if (existingTypes.Contains(fullTypeName))
                {
                    menu.AddDisabledItem(new GUIContent(FormatTypeName(fullTypeName)));
                    continue;
                }

                menu.AddItem(new GUIContent(FormatTypeName(fullTypeName)), false, () =>
                {
                    owner.RegisterCompleteObjectUndo("Add Processor");
                    node.ProcessorTypes.Add(fullTypeName);
                    listView.Rebuild();
                });
            }

            if (itemsProperty.arraySize == 0)
                menu.AddDisabledItem(new GUIContent("No processors available"));

            menu.ShowAsContext();
        }

        private static string FormatTypeName(string fullTypeName)
        {
            if (string.IsNullOrEmpty(fullTypeName)) return "(null)";

            int spaceIndex = fullTypeName.IndexOf(' ');
            if (spaceIndex < 0) return fullTypeName;

            string typeName = fullTypeName[(spaceIndex + 1)..];
            int dotIndex = typeName.LastIndexOf('.');
            return dotIndex >= 0 ? typeName[(dotIndex + 1)..] : typeName;
        }
    }
}