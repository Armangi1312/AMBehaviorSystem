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
        private const int SettingTypeIndex = 0;
        private const int ContextTypeIndex = 1;
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
            processorsProperty = serializedObject
                .FindProperty("<Processors>k__BackingField")
                .FindPropertyRelative("Items");
        }

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new();

            root.Add(new PropertyField(serializedObject.FindProperty("m_Script"), "Script") { enabledSelf = false });

            if (TryGetGenericType(ContextTypeIndex, out Type contextType))
            {
                RegistryField contextsField = new(
                    contextsProperty,
                    contextsProperty.FindPropertyRelative("<SerializedObjects>k__BackingField"),
                    contextType,
                    removalValidator: controller.IsContextRequired,
                    onChanged: InvokeValidateAndRepaint
                );
                contextsField.style.marginTop = 8;
                root.Add(contextsField);
            }

            if (TryGetGenericType(SettingTypeIndex, out Type settingType))
            {
                RegistryField settingsField = new(
                    settingsProperty,
                    settingsProperty.FindPropertyRelative("<SerializedObjects>k__BackingField"),
                    settingType,
                    removalValidator: controller.IsSettingRequired,
                    onChanged: InvokeValidateAndRepaint
                );
                settingsField.style.marginTop = 8;
                root.Add(settingsField);
            }

            if (TryGetGenericType(ProcessorTypeIndex, out Type processorType))
            {
                ProcessorListField processorListField = new(
                    processorsProperty,
                    processorType,
                    controller,
                    onChanged: InvokeValidateAndRepaint
                );
                processorListField.style.marginTop = 8;
                root.Add(processorListField);
            }

            return root;
        }

        private void InvokeValidateAndRepaint()
        {
            EditorUtility.SetDirty(controller);
            serializedObject.Update();
            EditorApplication.delayCall += () => Repaint();
        }

        private bool TryGetGenericType(int index, out Type type)
        {
            type = null;

            if (!GenericUtilities.TryResolveInheritedElementTypes(controller.GetType(), out Type[] types))
                return false;

            if (types.Length <= index)
                return false;

            type = types[index];
            return type != null;
        }
    }
}
