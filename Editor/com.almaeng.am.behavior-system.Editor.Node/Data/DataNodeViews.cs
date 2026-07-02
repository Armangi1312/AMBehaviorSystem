using AMBehaviorSystem.Core;
using AMBehaviorSystem.Node;
using AMBehaviorSystem.Node.Data;
using GraphProcessor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace AMBehaviorSystem.Editor.Node.Data
{
    public abstract class BaseControllerDataNodeView<TNode, TInterface> : BaseNodeView
        where TNode : BaseGetControllerDataNode
        where TInterface : class
    {
        private Label typeLabel;
        private Label pathLabel;
        protected TNode node;

        protected abstract string RowLabel { get; }
        protected abstract string UndoTypeLabel { get; }
        protected abstract string NoneFoundMessage { get; }
        protected abstract string SelectFirstMessage { get; }
        protected abstract string NotFoundMessage { get; }

        public override void Enable()
        {
            node = nodeTarget as TNode;

            typeLabel = NodeViewUIHelper.BuildEllipsisLabel(string.IsNullOrEmpty(node.Type) ? "(none)" : node.Type);
            pathLabel = NodeViewUIHelper.BuildEllipsisLabel(string.IsNullOrEmpty(node.Path) ? "(none)" : node.Path);

            controlsContainer.Add(NodeViewUIHelper.BuildRow(RowLabel, typeLabel, new Button(OnClickTypeButton) { text = "▾", style = { width = 24 } }));
            controlsContainer.Add(NodeViewUIHelper.BuildRow("Path", pathLabel, new Button(OnClickPathButton) { text = "▾", style = { width = 24 } }));
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
                menu.AddItem(new GUIContent(typeName), typeName == node.Type, () =>
                {
                    owner.RegisterCompleteObjectUndo(UndoTypeLabel);
                    node.Type = typeName;
                    node.Path = string.Empty;
                    typeLabel.text = typeName;
                    pathLabel.text = "(none)";
                });
            }
            menu.ShowAsContext();
        }

        private void OnClickPathButton()
        {
            if (string.IsNullOrEmpty(node.Type))
            {
                NodeViewUIHelper.ShowDisabledMenu(SelectFirstMessage);
                return;
            }

            TInterface target = FindItemByTypeName(node.Type);
            if (target == null)
            {
                NodeViewUIHelper.ShowDisabledMenu(NotFoundMessage);
                return;
            }

            List<string> paths = new();
            ReflectionPathUtility.CollectPaths(target.GetType(), string.Empty, paths, 0);

            NodeViewUIHelper.ShowPathMenu(paths, node.Path, path =>
            {
                owner.RegisterCompleteObjectUndo("Updated Node Path");
                node.Path = path;
                pathLabel.text = path;
            });
        }

        private List<TInterface> GetItemsFromGraph()
        {
            if (owner.graph is not NodeGraph nodeGraph) return null;
            nodeGraph.Resolve();
            Object controller = nodeGraph.TargetController;
            return controller == null ? null : ExtractFromController(controller);
        }

        private TInterface FindItemByTypeName(string typeName)
        {
            List<TInterface> items = GetItemsFromGraph();
            if (items == null) return null;
            foreach (TInterface item in items)
                if (item?.GetType().Name == typeName) return item;
            return null;
        }

        private static List<TInterface> ExtractFromController(Object controller)
        {
            List<TInterface> result = new();
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            Type controllerType = controller.GetType();

            foreach (PropertyInfo property in controllerType.GetProperties(flags))
            {
                if (!property.CanRead || !IsRegistryType(property.PropertyType)) continue;
                object registry = property.GetValue(controller);
                if (registry != null) CollectFromRegistry(registry, result);
            }

            foreach (FieldInfo field in controllerType.GetFields(flags))
            {
                if (!IsRegistryType(field.FieldType)) continue;
                object registry = field.GetValue(controller);
                if (registry != null) CollectFromRegistry(registry, result);
            }

            return result;
        }

        private static void CollectFromRegistry(object registry, List<TInterface> result)
        {
            PropertyInfo serializedObjectsProp = registry.GetType().GetProperty(
                "SerializedObjects", BindingFlags.Public | BindingFlags.Instance
            );
            if (serializedObjectsProp == null) return;
            if (serializedObjectsProp.GetValue(registry) is not IEnumerable enumerable) return;

            foreach (object item in enumerable)
                if (item is TInterface typed) result.Add(typed);
        }

        private static bool IsRegistryType(Type type)
        {
            while (type != null && type != typeof(object))
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Registry<>)) return true;
                type = type.BaseType;
            }
            return false;
        }
    }

    [NodeCustomEditor(typeof(GetContextDataNode))]
    public class GetContextDataNodeView : BaseControllerDataNodeView<GetContextDataNode, IContext>
    {
        protected override string RowLabel => "Context";
        protected override string UndoTypeLabel => "Updated Node Context Type";
        protected override string NoneFoundMessage => "(No IContext found on TargetController)";
        protected override string SelectFirstMessage => "(Select a Context type first)";
        protected override string NotFoundMessage => "(Context not found)";
    }

    [NodeCustomEditor(typeof(GetSettingDataNode))]
    public class GetSettingDataNodeView : BaseControllerDataNodeView<GetSettingDataNode, ISetting>
    {
        protected override string RowLabel => "Setting";
        protected override string UndoTypeLabel => "Updated Node Setting Type";
        protected override string NoneFoundMessage => "(No ISetting found on TargetController)";
        protected override string SelectFirstMessage => "(Select a Setting type first)";
        protected override string NotFoundMessage => "(Setting not found)";
    }

    [NodeCustomEditor(typeof(GetComponentDataNode))]
    public class GetComponentDataNodeView : BaseNodeView
    {
        private Label pathLabel;
        private GetComponentDataNode node;

        public override void Enable()
        {
            node = nodeTarget as GetComponentDataNode;

            ObjectField objectField = new()
            {
                label = "Target",
                objectType = typeof(GameObject),
                allowSceneObjects = true
            };

            objectField.RegisterValueChangedCallback(@event =>
            {
                owner.RegisterCompleteObjectUndo("Updated Node Target");
                node.Target.Value = @event.newValue as GameObject;
                UpdatePathLabel();
            });

            pathLabel = NodeViewUIHelper.BuildEllipsisLabel(string.IsNullOrEmpty(node.Path) ? "(none)" : node.Path);
            var pathButton = new Button(OnClickPathButton) { text = "▾", style = { width = 24 } };

            node.onProcessed += () =>
            {
                objectField.SetValueWithoutNotify(node.Target.Value);
                UpdatePathLabel();
            };

            schedule.Execute(() =>
            {
                node.Target.Resolve();
                objectField.SetValueWithoutNotify(node.Target.Value);
                UpdatePathLabel();
            });

            controlsContainer.Add(objectField);
            controlsContainer.Add(NodeViewUIHelper.BuildRow("Path", pathLabel, pathButton));
        }

        private void OnClickPathButton()
        {
            Object target = node.Target.Value;
            if (target == null) return;

            List<string> paths = new();

            if (target is GameObject gameObject)
            {
                foreach (Component component in gameObject.GetComponents<Component>())
                {
                    if (component == null) continue;
                    Type compType = component.GetType();
                    ReflectionPathUtility.CollectPaths(compType, compType.Name, paths, 0, blockUnityRefs: true);
                }
            }
            else
            {
                ReflectionPathUtility.CollectPaths(target.GetType(), string.Empty, paths, 0);
            }

            NodeViewUIHelper.ShowPathMenu(paths, node.Path, path =>
            {
                owner.RegisterCompleteObjectUndo("Updated Node Path");
                node.Path = path;
                UpdatePathLabel();
            });
        }

        private void UpdatePathLabel()
        {
            if (pathLabel == null) return;
            pathLabel.text = string.IsNullOrEmpty(node.Path) ? "(none)" : node.Path;
        }
    }
}
