using AMBehaviorSystem.Core;
using UnityEditor;
using UnityEngine.UIElements;

namespace AMBehaviorSystem.Editor
{
    [CustomPropertyDrawer(typeof(Registry<>), true)]
    public class RegistryDrawer : PropertyDrawer
    {
        private static readonly string BackingFieldName = "<SerializedObjects>k__BackingField";

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            SerializedProperty arrayProperty = property.FindPropertyRelative(BackingFieldName);

            if (arrayProperty == null)
                return new Label($"{property.displayName}: backing field not found");

            return new RegistryField(property, arrayProperty, fieldInfo.FieldType);
        }
    }
}