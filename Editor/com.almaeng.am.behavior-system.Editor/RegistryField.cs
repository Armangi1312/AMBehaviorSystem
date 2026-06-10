using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace AMBehaviorSystem.Editor
{
    internal sealed class RegistryField : BaseListField
    {
        private const BindingFlags InstanceDeclared = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;

        private readonly Func<Type, bool> removalValidator;

        public RegistryField(SerializedProperty registryProperty, SerializedProperty arrayProperty, Type registryType, Func<Type, bool> removalValidator = null)
            : base(arrayProperty, registryType, registryProperty.displayName)
        {
            this.removalValidator = removalValidator;

            int cachedSize = arrayProperty.arraySize;

            schedule.Execute(() =>
            {
                if (arrayProperty.serializedObject.targetObject == null) return;

                arrayProperty.serializedObject.Update();

                int currentSize = arrayProperty.arraySize;
                if (currentSize == cachedSize) return;

                cachedSize = currentSize;
                Refresh();
            }).Every(100);
        }

        protected override VisualElement MakeItem() => new Foldout { value = true };

        protected override void BindItem(VisualElement element, int index)
        {
            if (index >= arrayProperty.arraySize) return;

            SerializedProperty itemProperty = arrayProperty.GetArrayElementAtIndex(index);
            Foldout foldout = (Foldout)element;

            foldout.text = itemProperty.managedReferenceValue?.GetType().Name ?? "Null";
            foldout.Clear();

            DrawAllFields(foldout, itemProperty);
        }

        protected override void UnbindItem(VisualElement element, int index)
        {
            Foldout foldout = (Foldout)element;
            foldout.Unbind();
            foldout.Clear();
        }

        protected override bool CanRemove(int index)
        {
            if (removalValidator == null) return true;

            object item = arrayProperty.GetArrayElementAtIndex(index).managedReferenceValue;
            return !removalValidator(item?.GetType());
        }

        private void DrawAllFields(Foldout foldout, SerializedProperty itemProperty)
        {
            Type type = itemProperty.managedReferenceValue?.GetType();
            if (type == null) return;

            foreach (Type chainType in GenericUtilities.CollectTypeChain(type))
            {
                foreach (FieldInfo field in chainType.GetFields(InstanceDeclared))
                {
                    if (!IsSerializable(field)) continue;
                    AddPropertyField(foldout, itemProperty.FindPropertyRelative(field.Name));
                }

                foreach (PropertyInfo prop in chainType.GetProperties(InstanceDeclared))
                {
                    if (!prop.IsDefined(typeof(SerializeField), false)) continue;
                    string backingField = $"<{prop.Name}>k__BackingField";
                    AddPropertyField(foldout, itemProperty.FindPropertyRelative(backingField));
                }
            }
        }

        private static bool IsSerializable(FieldInfo field)
        {
            if (field.IsDefined(typeof(NonSerializedAttribute), false)) return false;
            if (field.IsPublic) return true;
            return field.IsDefined(typeof(SerializeField), false);
        }

        private void AddPropertyField(Foldout foldout, SerializedProperty prop)
        {
            if (prop == null) return;

            PropertyField propertyField = new(prop);
            propertyField.Bind(serializedObject);
            foldout.Add(propertyField);
        }
    }
}
