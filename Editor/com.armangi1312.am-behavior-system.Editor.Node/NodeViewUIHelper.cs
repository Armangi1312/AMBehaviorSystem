using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

namespace AMBehaviorSystem.Editor.Node
{
    internal static class NodeViewUIHelper
    {
        internal static VisualElement BuildRow(string labelText, VisualElement content, VisualElement button)
        {
            VisualElement row = new() { style = { flexDirection = FlexDirection.Row } };
            row.Add(new Label(labelText) { style = { width = 100, unityTextAlign = TextAnchor.MiddleLeft } });
            row.Add(content);
            row.Add(button);
            return row;
        }

        internal static Label BuildEllipsisLabel(string text) => new(text)
        {
            style =
            {
                flexGrow = 1,
                unityTextAlign = TextAnchor.MiddleLeft,
                overflow = Overflow.Hidden,
                textOverflow = TextOverflow.Ellipsis,
            }
        };

        internal static void ShowPathMenu(List<string> paths, string currentPath, System.Action<string> onSelect)
        {
            GenericMenu menu = new();

            if (paths == null || paths.Count == 0)
            {
                menu.AddDisabledItem(new GUIContent("(No selectable fields)"));
                menu.ShowAsContext();
                return;
            }

            foreach (string path in paths)
            {
                string captured = path;
                menu.AddItem(new GUIContent(captured.Replace('.', '/')), captured == currentPath, () => onSelect(captured));
            }

            menu.ShowAsContext();
        }

        internal static void ShowDisabledMenu(string message)
        {
            GenericMenu menu = new();
            menu.AddDisabledItem(new GUIContent(message));
            menu.ShowAsContext();
        }
    }
}
