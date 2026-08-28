using AMBehaviorSystem.Editor.Utilities;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace AMBehaviorSystem.Editor.UI
{
    public static class AMBSSettingsProvider
    {
        [SettingsProvider]
        public static SettingsProvider CreateMySettingsProvider()
        {
            SettingsProvider provider = new("Project/AM Behavior System", SettingsScope.Project)
            {
                label = "AM Behavior System",
                activateHandler = (context, root) =>
                {
                    string uxmlPath = FindAssetPath("AMBSSettings t:VisualTreeAsset");
                    string ussPath = FindAssetPath("AMBSSettings t:StyleSheet");

                    VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(uxmlPath);

                    if(visualTree == null)
                    {
                        Debug.LogError($"Failed to load VisualTreeAsset at path: {uxmlPath}");
                        return;
                    }

                    SerializedObject serializedObject = new(AMBSSettings.instance);
                    visualTree.CloneTree(root);

                    StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(ussPath);

                    if(styleSheet != null)
                    {
                        root.styleSheets.Add(styleSheet);
                    }

                    root.Bind(serializedObject);
                    root.RegisterCallback<SerializedPropertyChangeEvent>(_ => AMBSSettings.instance.Save());
                },
                keywords = new HashSet<string>(new[] { "AM", "Behavior", "System", "AMBS", "Settings" })
            };

            return provider;
        }

        private static string FindAssetPath(string searchFilter)
        {
            string[] guids = AssetDatabase.FindAssets(searchFilter);

            if(guids.Length == 0)
            {
                Debug.LogError($"Failed to find asset with filter: {searchFilter}");
                return null;
            }

            return AssetDatabase.GUIDToAssetPath(guids[0]);
        }
    }
}