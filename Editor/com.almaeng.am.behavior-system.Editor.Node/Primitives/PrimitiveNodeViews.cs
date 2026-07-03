using AMBehaviorSystem.Node.Base;
using AMBehaviorSystem.Node.Ports;
using AMBehaviorSystem.Node.Constants;
using GraphProcessor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AMBehaviorSystem.Editor.Node.Primitives
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

    [NodeCustomEditor(typeof(BooleanNode))]
    public class BooleanNodeView : BaseValueNodeView<BooleanNode, Toggle, bool, BooleanPort> { }

    [NodeCustomEditor(typeof(IntegerNode))]
    public class IntegerNodeView : BaseValueNodeView<IntegerNode, IntegerField, int, NumberPort> { }

    [NodeCustomEditor(typeof(FloatNode))]
    public class FloatNodeView : BaseValueNodeView<FloatNode, FloatField, float, NumberPort> { }

    [NodeCustomEditor(typeof(DoubleNode))]
    public class DoubleNodeView : BaseValueNodeView<DoubleNode, DoubleField, double, NumberPort> { }

    [NodeCustomEditor(typeof(Vector2Node))]
    public class Vector2NodeView : BaseValueNodeView<Vector2Node, Vector2Field, Vector2, Vector2Port> { }

    [NodeCustomEditor(typeof(Vector3Node))]
    public class Vector3NodeView : BaseValueNodeView<Vector3Node, Vector3Field, Vector3, Vector3Port> { }

    [NodeCustomEditor(typeof(Vector4Node))]
    public class Vector4NodeView : BaseValueNodeView<Vector4Node, Vector4Field, Vector4, Vector4Port> { }
}
