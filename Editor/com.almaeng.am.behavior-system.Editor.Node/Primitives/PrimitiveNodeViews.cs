using AMBehaviorSystem.Node.Ports;
using AMBehaviorSystem.Node.Primitives;
using GraphProcessor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AMBehaviorSystem.Editor.Node.Primitives
{
    public abstract class PrimitiveNodeView<TNode, TField, TValue, TPort> : BaseNodeView
        where TNode : BasePrimitiveNode<TValue, TPort>
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

    [NodeCustomEditor(typeof(BooleanNode))]
    public class BooleanNodeView : PrimitiveNodeView<BooleanNode, Toggle, bool, BooleanPort> { }

    [NodeCustomEditor(typeof(IntegerNode))]
    public class IntegerNodeView : PrimitiveNodeView<IntegerNode, IntegerField, int, NumberPort> { }

    [NodeCustomEditor(typeof(FloatNode))]
    public class FloatNodeView : PrimitiveNodeView<FloatNode, FloatField, float, NumberPort> { }

    [NodeCustomEditor(typeof(DoubleNode))]
    public class DoubleNodeView : PrimitiveNodeView<DoubleNode, DoubleField, double, NumberPort> { }

    [NodeCustomEditor(typeof(Vector2Node))]
    public class Vector2NodeView : PrimitiveNodeView<Vector2Node, Vector2Field, Vector2, Vector2Port> { }

    [NodeCustomEditor(typeof(Vector3Node))]
    public class Vector3NodeView : PrimitiveNodeView<Vector3Node, Vector3Field, Vector3, Vector3Port> { }

    [NodeCustomEditor(typeof(Vector4Node))]
    public class Vector4NodeView : PrimitiveNodeView<Vector4Node, Vector4Field, Vector4, Vector4Port> { }
}
