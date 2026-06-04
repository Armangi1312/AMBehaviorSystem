using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace AMBehaviorSystem.Editor
{
    internal sealed class RegistryField : VisualElement
    {
        private const BindingFlags PublicInstanceDeclared = BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly;
        private const BindingFlags NonPublicInstanceDeclared = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
        private const BindingFlags AllInstanceDeclared = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;

        private readonly SerializedProperty arrayProperty;
        private readonly SerializedObject serializedObject;
        private readonly ListView listView;
        private readonly List<Type> registeredTypes = new();
        private readonly List<int> indexSource = new();
        private readonly Type[] candidateTypes;

        public RegistryField(SerializedProperty registryProperty, SerializedProperty arrayProperty, Type registryType)
        {
            this.arrayProperty = arrayProperty;
            serializedObject = registryProperty.serializedObject;

            LoadStyleSheet();

            candidateTypes = GenericUtilities.TryResolveElementTypes(registryType, out Type[] elementTypes)
                ? GenericUtilities.CollectInheritedTypes(elementTypes[0])
                : Array.Empty<Type>();

            RebuildSources();

            Add(BuildHeader(registryProperty.displayName));
            listView = BuildListView();
            Add(listView);
        }

        private void LoadStyleSheet()
        {
            string[] guids = AssetDatabase.FindAssets("HeaderListViewStyle t:StyleSheet");
            if (guids.Length == 0) return;

            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(path);
            if (styleSheet != null)
                styleSheets.Add(styleSheet);
        }

        private static VisualElement BuildHeader(string displayName)
        {
            VisualElement header = new();
            header.AddToClassList("listview-header");

            Label label = new(displayName);
            label.AddToClassList("listview-header-label");
            header.Add(label);

            return header;
        }

        private ListView BuildListView()
        {
            ListView lv = new()
            {
                reorderable = true,
                showFoldoutHeader = false,
                showAddRemoveFooter = true,
                showBorder = true,
                reorderMode = ListViewReorderMode.Animated,
                virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight,
                itemsSource = indexSource,
                makeItem = MakeItem,
                bindItem = BindItem,
                unbindItem = UnbindItem,
                overridingAddButtonBehavior = (_, _) => ShowAddMenu(),
                onRemove = _ => OnRemoveClicked()
            };

            lv.itemIndexChanged += OnItemIndexChanged;

            return lv;
        }

        private void OnItemIndexChanged(int oldIndex, int newIndex)
        {
            serializedObject.Update();
            arrayProperty.MoveArrayElement(oldIndex, newIndex);
            serializedObject.ApplyModifiedProperties();
            Refresh();
        }

        private static VisualElement MakeItem() => new Foldout { value = true };

        private void BindItem(VisualElement element, int index)
        {
            if (index >= arrayProperty.arraySize) return;

            SerializedProperty itemProperty = arrayProperty.GetArrayElementAtIndex(index);
            Foldout foldout = (Foldout)element;

            foldout.text = itemProperty.managedReferenceValue?.GetType().Name ?? "Null";
            foldout.Clear();

            DrawAllFields(foldout, itemProperty);
        }

        private void UnbindItem(VisualElement element, int _)
        {
            Foldout foldout = (Foldout)element;
            foldout.Unbind();
            foldout.Clear();
        }

        private void DrawAllFields(Foldout foldout, SerializedProperty itemProperty)
        {
            Type type = itemProperty.managedReferenceValue?.GetType();
            if (type == null) return;

            foreach (Type t in CollectTypeChain(type))
            {
                foreach (FieldInfo field in t.GetFields(PublicInstanceDeclared))
                {
                    if (field.IsDefined(typeof(NonSerializedAttribute), false)) continue;
                    AddPropertyField(foldout, itemProperty.FindPropertyRelative(field.Name));
                }

                foreach (FieldInfo field in t.GetFields(NonPublicInstanceDeclared))
                {
                    if (!field.IsDefined(typeof(SerializeField), false)) continue;
                    AddPropertyField(foldout, itemProperty.FindPropertyRelative(field.Name));
                }

                foreach (PropertyInfo prop in t.GetProperties(AllInstanceDeclared))
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

        private void ShowAddMenu()
        {
            GenericMenu menu = new();
            bool any = false;

            foreach (Type type in candidateTypes)
            {
                if (registeredTypes.Contains(type)) continue;

                any = true;
                Type cached = type;
                menu.AddItem(new GUIContent(type.Name), false, () => AddItem(cached));
            }

            if (!any)
                menu.AddDisabledItem(new GUIContent("No compatible types found"));

            menu.ShowAsContext();
        }

        private void AddItem(Type type)
        {
            if (registeredTypes.Contains(type)) return;

            serializedObject.Update();

            int index = arrayProperty.arraySize;
            arrayProperty.InsertArrayElementAtIndex(index);
            arrayProperty.GetArrayElementAtIndex(index).managedReferenceValue = Activator.CreateInstance(type);

            serializedObject.ApplyModifiedProperties();
            Refresh();
        }

        private void OnRemoveClicked()
        {
            int targetIndex = listView.selectedIndex >= 0
                ? listView.selectedIndex
                : arrayProperty.arraySize - 1;

            if (targetIndex < 0 || targetIndex >= arrayProperty.arraySize) return;

            serializedObject.Update();
            arrayProperty.DeleteArrayElementAtIndex(targetIndex);
            serializedObject.ApplyModifiedProperties();
            Refresh();
        }

        private void Refresh()
        {
            RebuildSources();
            listView.RefreshItems();
        }

        private void RebuildSources()
        {
            indexSource.Clear();
            registeredTypes.Clear();

            for (int i = 0; i < arrayProperty.arraySize; i++)
            {
                indexSource.Add(i);

                object item = arrayProperty.GetArrayElementAtIndex(i).managedReferenceValue;
                if (item != null)
                    registeredTypes.Add(item.GetType());
            }
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
    }
}
