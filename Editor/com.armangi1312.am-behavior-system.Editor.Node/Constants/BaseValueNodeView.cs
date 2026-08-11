using AMBehaviorSystem.Node;
using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using UnityEngine.UIElements;

namespace AMBehaviorSystem.Editor.Node.Constants
{
    public abstract class BaseValueNodeView<TNode, TField, TValue, TPort> : BaseNodeView
        where TNode : BaseValueNode<TValue, TPort>
        where TField : BaseField<TValue>, new()
        where TValue : struct
        where TPort : Port
    {
        public override void Enable()
        {
            TNode node = (TNode)nodeTarget;
            TField field = new() { value = node.Field };

            node.onProcessed += () => field.value = node.Field;
            field.RegisterValueChangedCallback(e =>
            {
                owner.RegisterCompleteObjectUndo($"Updated {typeof(TNode).Name} Value");
                node.Field = e.newValue;
            });

            controlsContainer.Add(field);
        }
    }
}
