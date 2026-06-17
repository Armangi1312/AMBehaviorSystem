using AMBehaviorSystem.Node.Data;
using GraphProcessor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace AMBehaviorSystem.Editor.Node.Data
{
    [NodeCustomEditor(typeof(GetComponentDataNode))]
    public class GetComponentDataNodeView : BaseNodeView
    {
        public override void Enable()
        {
            GetComponentDataNode node = nodeTarget as GetComponentDataNode;

            ObjectField objectField = new()
            {
                label = "Target",
                objectType = typeof(Object),
                allowSceneObjects = true
            };

            objectField.RegisterValueChangedCallback(@event =>
            {
                owner.RegisterCompleteObjectUndo("Updated Node");
                node.Target.Value = @event.newValue;
            });

            node.onProcessed += () => objectField.SetValueWithoutNotify(node.Target.Value);
            
            schedule.Execute(() =>
            {
                node.Target.Resolve();
                objectField.SetValueWithoutNotify(node.Target.Value);
            });

            controlsContainer.Add(objectField);


        }
    }
}
