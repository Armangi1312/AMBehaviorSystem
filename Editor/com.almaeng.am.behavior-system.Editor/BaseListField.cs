using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AMBehaviorSystem.Editor
{
    internal abstract class BaseListField : VisualElement
    {
        protected readonly SerializedProperty arrayProperty;
        protected readonly SerializedObject serializedObject;
        protected readonly List<Type> registeredTypes = new();
        protected readonly List<int> indexSource = new();
        protected readonly Type[] candidateTypes;
        protected ListView listView;

        protected BaseListField(SerializedProperty arrayProperty, Type elementType, string headerLabel)
        {
            this.arrayProperty = arrayProperty;
            serializedObject = arrayProperty.serializedObject;
            candidateTypes = GenericUtilities.CollectInheritedTypes(elementType);

            LoadStyleSheet();
            RebuildSources();

            Add(BuildHeader(headerLabel));
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

        private static VisualElement BuildHeader(string label)
        {
            VisualElement header = new();
            header.AddToClassList("listview-header");

            Label headerLabel = new(label);
            headerLabel.AddToClassList("listview-header-label");
            header.Add(headerLabel);

            return header;
        }

        private ListView BuildListView()
        {
            var lv = new ListView
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
                onRemove = _ => OnRemoveClicked(),
            };

            lv.itemIndexChanged += OnItemIndexChanged;
            lv.style.flexGrow = 1;
            lv.style.minHeight = 20;

            return lv;
        }

        private void OnItemIndexChanged(int oldIndex, int newIndex)
        {
            serializedObject.Update();
            arrayProperty.MoveArrayElement(oldIndex, newIndex);
            serializedObject.ApplyModifiedProperties();
            Refresh();
        }

        private void ShowAddMenu()
        {
            GenericMenu menu = new();
            bool hasAny = false;

            foreach (Type type in candidateTypes)
            {
                if (registeredTypes.Contains(type)) continue;

                hasAny = true;
                Type captured = type;
                menu.AddItem(new GUIContent(type.Name), false, () => AddItem(captured));
            }

            if (!hasAny)
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
            OnAfterAdd();
        }

        private void OnRemoveClicked()
        {
            int targetIndex = listView.selectedIndex >= 0
                ? listView.selectedIndex
                : arrayProperty.arraySize - 1;

            if (targetIndex < 0 || targetIndex >= arrayProperty.arraySize) return;
            if (!CanRemove(targetIndex)) return;

            serializedObject.Update();
            arrayProperty.DeleteArrayElementAtIndex(targetIndex);
            serializedObject.ApplyModifiedProperties();
            OnAfterRemove();
        }

        protected void Refresh()
        {
            RebuildSources();
            listView.RefreshItems();
        }

        private void RebuildSources()
        {
            serializedObject.Update();
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

        protected abstract VisualElement MakeItem();
        protected abstract void BindItem(VisualElement element, int index);
        protected abstract void UnbindItem(VisualElement element, int index);

        protected virtual bool CanRemove(int index) => true;
        protected virtual void OnAfterAdd() => Refresh();
        protected virtual void OnAfterRemove() => Refresh();
    }
}
