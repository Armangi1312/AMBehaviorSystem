using AMBehaviorSystem.Core;
using System;
using UnityEditor;
using UnityEngine.UIElements;

namespace AMBehaviorSystem.Editor
{
    internal sealed class ProcessorListField : BaseListField
    {
        private readonly Controller controller;

        public ProcessorListField(SerializedProperty arrayProperty, Type processorType, Controller controller, Action onChanged = null)
            : base(arrayProperty, processorType, "Processors", onChanged)
        {
            this.controller = controller;
        }

        protected override VisualElement MakeItem()
        {
            Label label = new();
            label.style.paddingLeft = 4;
            label.style.paddingTop = 2;
            label.style.paddingBottom = 2;
            return label;
        }

        protected override void BindItem(VisualElement element, int index)
        {
            if (index >= arrayProperty.arraySize) return;

            SerializedProperty itemProperty = arrayProperty.GetArrayElementAtIndex(index);
            ((Label)element).text = itemProperty.managedReferenceValue?.GetType().Name ?? "Null";
        }

        protected override void UnbindItem(VisualElement element, int index)
        {
            ((Label)element).text = string.Empty;
        }

        protected override void OnAfterAdd()
        {
            MarkControllerDirty();
        }

        protected override void OnAfterRemove()
        {
            MarkControllerDirty();
        }

        private void MarkControllerDirty()
        {
            if (controller == null) return;
            EditorUtility.SetDirty(controller);
            serializedObject.Update();
        }
    }
}
