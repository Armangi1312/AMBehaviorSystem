using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace AMBehaviorSystem.Editor
{
    internal sealed class RegistryField : BaseListField
    {
        private const BindingFlags PublicInstanceDeclared = BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly;
        private const BindingFlags NonPublicInstanceDeclared = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
        private const BindingFlags AllInstanceDeclared = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;

        private readonly Func<Type, bool> removalValidator;

        public RegistryField(SerializedProperty registryProperty, SerializedProperty arrayProperty, Type registryType, Func<Type, bool> removalValidator = null)
            : base(arrayProperty, ResolveElementType(registryType), registryProperty.displayName)
        {
            this.removalValidator = removalValidator;

            this.TrackSerializedObjectValue(serializedObject, @object =>
            {
                Refresh();
            });
        }

        private static Type ResolveElementType(Type registryType)
        {
            return GenericUtilities.TryResolveElementTypes(registryType, out Type[] elementTypes)
                ? elementTypes[0]
                : null;
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

            foreach (Type chainType in CollectTypeChain(type))
            {
                foreach (FieldInfo field in chainType.GetFields(PublicInstanceDeclared))
                {
                    if (field.IsDefined(typeof(NonSerializedAttribute), false)) continue;
                    AddPropertyField(foldout, itemProperty.FindPropertyRelative(field.Name));
                }

                foreach (FieldInfo field in chainType.GetFields(NonPublicInstanceDeclared))
                {
                    if (!field.IsDefined(typeof(SerializeField), false)) continue;
                    AddPropertyField(foldout, itemProperty.FindPropertyRelative(field.Name));
                }

                foreach (PropertyInfo prop in chainType.GetProperties(AllInstanceDeclared))
                {
                    if (!prop.IsDefined(typeof(SerializeField), false)) continue;
                    string backingFieldName = $"<{prop.Name}>k__BackingField";
                    AddPropertyField(foldout, itemProperty.FindPropertyRelative(backingFieldName));
                }
            }
        }

        private void AddPropertyField(Foldout foldout, SerializedProperty prop)
        {
            if (prop == null) return;

            PropertyField propertyField = new(prop);
            propertyField.Bind(serializedObject);
            foldout.Add(propertyField);
        }

        private static List<Type> CollectTypeChain(Type type)
        {
            List<Type> chain = new();
            Type current = type;

            while (current != null && current != typeof(object))
            {
                chain.Insert(0, current);
                current = current.BaseType;
            }

            return chain;
        }

        protected override void OnAfterAdd()
        {
            Refresh();
        }

        protected override void OnAfterRemove()
        {
            Refresh();
        }
    }
}
