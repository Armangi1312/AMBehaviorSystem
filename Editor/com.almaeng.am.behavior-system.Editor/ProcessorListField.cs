using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AMBehaviorSystem.Editor
{
    internal sealed class ProcessorListField : VisualElement
    {
        private readonly SerializedProperty arrayProperty;
        private readonly SerializedObject serializedObject;
        private readonly ListView listView;
        private readonly List<Type> registeredTypes = new();
        private readonly List<int> indexSource = new();
        private readonly Type[] candidateTypes;

        public ProcessorListField(SerializedProperty processorProperty, SerializedProperty arrayProperty, Type processorType)
        {
            this.arrayProperty = arrayProperty;
            serializedObject = processorProperty.serializedObject;

            LoadStyleSheet();

            candidateTypes = GenericUtilities.CollectInheritedTypes(processorType);

            RebuildSources();

            Add(BuildHeader());
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

        private static VisualElement BuildHeader()
        {
            VisualElement header = new();
            header.AddToClassList("listview-header");

            Label label = new("Processors");
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

        private static VisualElement MakeItem()
        {
            Label label = new();
            label.style.paddingLeft = 4;
            label.style.paddingTop = 2;
            label.style.paddingBottom = 2;
            return label;
        }

        private void BindItem(VisualElement element, int index)
        {
            if (index >= arrayProperty.arraySize) return;

            SerializedProperty itemProperty = arrayProperty.GetArrayElementAtIndex(index);
            ((Label)element).text = itemProperty.managedReferenceValue?.GetType().Name ?? "Null";
        }

        private static void UnbindItem(VisualElement element, int _)
        {
            ((Label)element).text = string.Empty;
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
    }
}
