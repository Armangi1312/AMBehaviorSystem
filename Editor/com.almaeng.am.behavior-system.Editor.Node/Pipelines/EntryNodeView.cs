using AMBehaviorSystem.Core;
using AMBehaviorSystem.Node;
using AMBehaviorSystem.Node.Pipelines;
using GraphProcessor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace AMBehaviorSystem.Editor.Pipelines
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

            node.onProcessed += () => objectField.SetValueWithoutNotify(Graph?.TargetController);

            schedule.Execute(() =>
            {
                Graph?.Resolve();
                objectField.SetValueWithoutNotify(Graph?.TargetController);
            });

            controlsContainer.Add(objectField);
        }
    }
}