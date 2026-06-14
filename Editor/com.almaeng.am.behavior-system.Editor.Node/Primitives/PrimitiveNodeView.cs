using AMBehaviorSystem.Node.Primitives;
using GraphProcessor;
using UnityEngine.UIElements;

namespace AMBehaviorSystem.Editor.Node
{
    public abstract class PrimitiveNodeView<TNode, TField, TValue> : BaseNodeView
        where TNode : BasePrimitiveNode<TValue>
        where TField : BaseField<TValue>, new()
        where TValue : struct
    {
        public override void Enable()
        {
            TNode node = nodeTarget as TNode;
            TField field = new() { value = node.Field };

            node.onProcessed += () => field.value = node.Field;
            field.RegisterValueChangedCallback(e =>
            {
                owner.RegisterCompleteObjectUndo($"Updated {typeof(TNode).Name} input");
                node.Field = e.newValue;
            });

            controlsContainer.Add(field);
        }
    }
}
