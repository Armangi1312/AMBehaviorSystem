using AMBehaviorSystem.Node.Primitives;
using GraphProcessor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AMBehaviorSystem.Editor.Node.Primitives
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

    [NodeCustomEditor(typeof(BooleanNode))]
    public class BooleanNodeView : PrimitiveNodeView<BooleanNode, Toggle, bool> { }

    [NodeCustomEditor(typeof(IntegerNode))]
    public class IntegerNodeView : PrimitiveNodeView<IntegerNode, IntegerField, int> { }

    [NodeCustomEditor(typeof(FloatNode))]
    public class FloatNodeView : PrimitiveNodeView<FloatNode, FloatField, float> { }

    [NodeCustomEditor(typeof(DoubleNode))]
    public class DoubleNodeView : PrimitiveNodeView<DoubleNode, DoubleField, double> { }

    [NodeCustomEditor(typeof(Vector2Node))]
    public class Vector2NodeView : PrimitiveNodeView<Vector2Node, Vector2Field, Vector2> { }

    [NodeCustomEditor(typeof(Vector3Node))]
    public class Vector3NodeView : PrimitiveNodeView<Vector3Node, Vector3Field, Vector3> { }

    [NodeCustomEditor(typeof(Vector4Node))]
    public class Vector4NodeView : PrimitiveNodeView<Vector4Node, Vector4Field, Vector4> { }
}
