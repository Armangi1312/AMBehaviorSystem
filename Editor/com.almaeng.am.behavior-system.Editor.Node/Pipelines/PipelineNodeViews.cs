using AMBehaviorSystem.Core;
using AMBehaviorSystem.Node;
using AMBehaviorSystem.Node.Pipelines;
using GraphProcessor;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace AMBehaviorSystem.Editor.Node.Pipelines
{
    [NodeCustomEditor(typeof(EntryNode))]
    public class EntryNodeView : BaseNodeView
    {
        private NodeGraph Graph => owner.graph as NodeGraph;

        public override void Enable()
        {
            EntryNode node = nodeTarget as EntryNode;

            ObjectField objectField = new()
            {
                objectType = typeof(Controller),
                allowSceneObjects = true
            };

            objectField.RegisterValueChangedCallback(@event =>
            {
                owner.RegisterCompleteObjectUndo("Updated EntryNode");
                Graph.TargetController = @event.newValue as Controller;
            });

            if (Graph != null)
            {
                node.onProcessed += () => objectField.SetValueWithoutNotify(Graph.TargetController);

                schedule.Execute(() =>
                {
                    Graph.Resolve();
                    objectField.SetValueWithoutNotify(Graph.TargetController);
                });
            }

            controlsContainer.Add(objectField);
        }
    }

    [NodeCustomEditor(typeof(InvokeNode))]
    public class InvokeNodeView : BaseNodeView
    {
        private NodeGraph Graph => owner.graph as NodeGraph;
        private InvokeNode Node => nodeTarget as InvokeNode;

        public override void Enable()
        {
            var listView = new ListView
            {
                reorderable = false,
                showAddRemoveFooter = true,
                showBorder = true,
                showFoldoutHeader = false,
                itemsSource = Node.ProcessorTypes,
                makeItem = () => new Label(),
                bindItem = (element, i) =>
                {
                    Label label = element as Label;
                    label.text = FormatTypeName(Node.ProcessorTypes[i]);
                }
            };

            listView.itemsRemoved += _ =>
            {
                owner.RegisterCompleteObjectUndo("Remove Processor");
                listView.Rebuild();
            };

            listView.onAdd = _ => ShowAddMenu(listView);

            controlsContainer.Add(listView);
        }

        private void ShowAddMenu(ListView listView)
        {
            var controller = Graph?.TargetController;
            if (controller == null) return;

            var serializedObject = new SerializedObject(controller);
            var processorsProperty = serializedObject.FindProperty("<Processors>k__BackingField");
            var itemsProperty = processorsProperty?.FindPropertyRelative("Items");
            if (itemsProperty == null) return;

            var menu = new GenericMenu();
            var existing = new HashSet<string>(Node.ProcessorTypes);

            for (int i = 0; i < itemsProperty.arraySize; i++)
            {
                var element = itemsProperty.GetArrayElementAtIndex(i);
                string fullTypeName = element.managedReferenceFullTypename;

                if (string.IsNullOrEmpty(fullTypeName)) continue;

                if (existing.Contains(fullTypeName))
                {
                    menu.AddDisabledItem(new GUIContent(FormatTypeName(fullTypeName)));
                    continue;
                }

                menu.AddItem(new GUIContent(FormatTypeName(fullTypeName)), false, () =>
                {
                    owner.RegisterCompleteObjectUndo("Add Processor");
                    Node.ProcessorTypes.Add(fullTypeName);
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
