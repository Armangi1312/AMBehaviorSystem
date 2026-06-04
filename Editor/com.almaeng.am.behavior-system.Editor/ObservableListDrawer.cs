using AMBehaviorSystem.Core.Utilities;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace AMBehaviorSystem.Editor
{
    [CustomPropertyDrawer(typeof(ObservableList<>), true)]
    public class ObservableListDrawer : PropertyDrawer
    {
        private static readonly string ItemsFieldName = "Items";

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            SerializedProperty arrayProperty = property.FindPropertyRelative(ItemsFieldName);

            if (arrayProperty == null)
                return new Label($"{property.displayName}: Items field not found");

            return new PropertyField(arrayProperty, property.displayName);
        }
    }
}
