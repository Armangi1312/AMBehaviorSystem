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

            if (target is GameObject gameObject)
            {
                foreach (Component component in gameObject.GetComponents<Component>())
                {
                    if (component == null) continue;

                    Type componentType = component.GetType();
                    ReflectionPathUtility.CollectPaths(componentType, componentType.Name, paths, 0, blockUnityRefs: true);
                }
            }
            else
            {
                ReflectionPathUtility.CollectPaths(target.GetType(), string.Empty, paths, 0);
            }

            return paths;
        }
    }
}