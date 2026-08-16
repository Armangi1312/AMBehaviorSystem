using AMBehaviorSystem.Editor.Node;
using AMBehaviorSystem.Editor.Utilities;
using AMBehaviorSystem.Node;
using AMBehaviorSystem.Node.Data;
using GraphProcessor;
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AMBehaviorSystem.Editor.Node.Data
{
    [NodeCustomEditor(typeof(ComponentDataNode))]
    internal class ComponentDataNodeView : BaseDataNodeView<ComponentDataNode>
    {
        private NodeGraph Graph => owner.graph as NodeGraph;
        private readonly Dictionary<string, Type> componentTypeMap = new();

        protected override string CannotSelectPathMessage => "(No target found)";

        protected override void OnEnabled()
        {
            Node.onProcessed += UpdatePathLabel;
            schedule.Execute(UpdatePathLabel);
        }

        protected override bool CanSelectPath() => Graph?.TargetController != null;

        protected override List<PathEntry> BuildPathOptions()
        {
            Object target = Graph.TargetController;
            List<PathEntry> entries = new();
            componentTypeMap.Clear();

            if(target is Component gameObject)
            {
                foreach(Component component in gameObject.GetComponentsInChildren<Component>())
                {
                    if(component == null)
                        continue;

                    Type componentType = component.GetType();
                    componentTypeMap[componentType.Name] = componentType;
                    ReflectionPathUtilities.CollectPaths(componentType, componentType.Name, entries, 0, blockUnityRefs: false);
                }
            }
            else
            {
                Type targetType = target.GetType();
                componentTypeMap[string.Empty] = targetType;
                ReflectionPathUtilities.CollectPaths(targetType, string.Empty, entries, 0);
            }

            return entries;
        }

        protected override void OnSelected(string path)
        {
            const char Separator = '.';

            if(string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Path cannot be null or empty.", nameof(path));
            }

            int separatorIndex = path.IndexOf(Separator);
            if(separatorIndex < 0)
            {
                throw new FormatException($"Path '{path}' does not contain a member (expected format: 'Type.member').");
            }

            string componentTypeName = path.Substring(0, separatorIndex);
            string memberPath = path.Substring(separatorIndex + 1);

            if(memberPath.Length == 0)
            {
                throw new FormatException($"Path '{path}' has no member after separator.");
            }

            Node.TypeName = componentTypeMap.TryGetValue(componentTypeName, out Type componentType)
                ? componentType.AssemblyQualifiedName
                : string.Empty;
            Node.Path = memberPath;
        }
    }
}