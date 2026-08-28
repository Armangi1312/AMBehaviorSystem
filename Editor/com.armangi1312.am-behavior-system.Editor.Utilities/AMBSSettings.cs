using System;
using UnityEditor;
using UnityEngine;

namespace AMBehaviorSystem.Editor.Utilities
{
    [Serializable]
    [FilePath("ProjectSettings/AMBehaviorSystemSettings.asset", FilePathAttribute.Location.ProjectFolder)]
    public class AMBSSettings : ScriptableSingleton<AMBSSettings>
    {
        [field: SerializeField] public bool UpdateCheck { get; set; } = true;
        [field: SerializeField] public bool AutoUpdate { get; set; } = true;

        [field: SerializeField] public string SourceGenerationPath { get; set; } = "Generated";
        [field: SerializeField] public string SourceGenerationNamespace { get; set; } = "AMBehaviorSystem.Generated";

        public void Save() => Save(true);
    }
}
