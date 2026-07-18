using AMBehaviorSystem.Node;
using AMBehaviorSystem.Node.Data;
using GraphProcessor;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AMBehaviorSystem.Editor.Node.Data
{
    [NodeCustomEditor(typeof(ComponentDataNode))]
    public class ComponentDataNodeView : BaseDataNodeView<ComponentDataNode>
    {
        private NodeGraph Graph => owner.graph as NodeGraph;

        protected override string CannotSelectPathMessage => "(No target found)";

        protected override void OnEnabled()
        {
            Node.onProcessed += UpdatePathLabel;
            schedule.Execute(UpdatePathLabel);
        }

        protected override bool CanSelectPath() => Graph?.TargetController != null;

        protected override List<string> BuildPathOptions()
        {
            Object target = Graph.TargetController;
            List<string> paths = new();

            if (target is Component gameObject)
            {
                foreach (Component component in gameObject.GetComponentsInChildren<Component>())
                {
                    if (component == null) continue;

                    Type componentType = component.GetType();
                    ReflectionPathUtility.CollectPaths(componentType, componentType.Name, paths, 0, blockUnityRefs: false);
                }
            }
            else
            {
                ReflectionPathUtility.CollectPaths(target.GetType(), string.Empty, paths, 0);
            }

            return paths;
        }

        protected override void OnSelected(string path)
        {
            const char Separator = '.';

            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Path cannot be null or empty.", nameof(path));
            }

            int separatorIndex = path.IndexOf(Separator);
            if (separatorIndex < 0)
            {
                throw new FormatException($"Path '{path}' does not contain a member (expected format: 'Type.member').");
            }

            string componentTypeName = path.Substring(0, separatorIndex);
            string memberPath = path.Substring(separatorIndex + 1);

            if (memberPath.Length == 0)
            {
                throw new FormatException($"Path '{path}' has no member after separator.");
            }

            Node.Type = componentTypeName;
            Node.Path = memberPath;

            Debug.Log($"{Node.Type}.{Node.Path}");
        }
    }
}