using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace AMBehaviorSystem.Editor.UI
{
    public static class AMBSSettingsProvider
    {
        private const string UxmlPath = "Packages/com.armangi1312.am-behavior-system/Editor/com.armangi1312.am-behavior-system.Editor/UI/AMBSSettings.uxml";
        private const string UssPath = "Packages/com.armangi1312.am-behavior-system/Editor/com.armangi1312.am-behavior-system.Editor/UI/AMBSSettings.uss";

        [SettingsProvider]
        public static SettingsProvider CreateMySettingsProvider()
        {
            SettingsProvider provider = new("Project/AM Behavior System", SettingsScope.Project)
            {
                label = "AM Behavior System",
                activateHandler = (context, root) =>
                {
                    VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UxmlPath);

                    if (visualTree == null)
                    {
                        Debug.LogError($"Failed to load VisualTreeAsset at path: {UxmlPath}");
                        return;
                    }

                    SerializedObject serializedObject = new(AMBSSettings.instance);
                    visualTree.CloneTree(root);

                    StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(UssPath);

                    if (styleSheet != null)
                    {
                        root.styleSheets.Add(styleSheet);
                    }

                    root.Bind(serializedObject);
                },
                keywords = new HashSet<string>(new[] { "AM", "Behavior", "System", "AMBS", "Settings" })
            };

            return provider;
        }
    }
}