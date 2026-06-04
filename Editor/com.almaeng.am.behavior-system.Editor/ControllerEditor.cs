using AMBehaviorSystem.Core;
using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace AMBehaviorSystem.Editor
{
    [CustomEditor(typeof(Controller), true)]
    internal class ControllerEditor : UnityEditor.Editor
    {
        private const int ProcessorTypeIndex = 2;

        private Controller controller;
        private SerializedProperty settingsProperty;
        private SerializedProperty contextsProperty;
        private SerializedProperty processorsProperty;

        private void OnEnable()
        {
            controller = target as Controller;

            settingsProperty = serializedObject.FindProperty("<Settings>k__BackingField");
            contextsProperty = serializedObject.FindProperty("<Contexts>k__BackingField");
            processorsProperty = serializedObject.FindProperty("<Processors>k__BackingField").FindPropertyRelative("Items");
        }

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new();

            root.Add(new PropertyField(serializedObject.FindProperty("m_Script"), "Script") { enabledSelf = false });

            PropertyField settingsField = new(settingsProperty);
            settingsField.style.marginTop = 8;
            root.Add(settingsField);

            PropertyField contextsField = new(contextsProperty);
            contextsField.style.marginTop = 8;
            root.Add(contextsField);

            if (TryGetProcessorType(out Type processorType))
            {
                ProcessorListField processorListField = new(processorsProperty, processorsProperty, processorType);
                processorListField.style.marginTop = 8;
                root.Add(processorListField);
            }

            return root;
        }

        private bool TryGetProcessorType(out Type processorType)
        {
            processorType = null;

            if (!GenericUtilities.TryResolveInheritedElementTypes(controller.GetType(), out Type[] types))
                return false;

            if (types.Length <= ProcessorTypeIndex)
                return false;

            processorType = types[ProcessorTypeIndex];
            return processorType != null;
        }
    }
}