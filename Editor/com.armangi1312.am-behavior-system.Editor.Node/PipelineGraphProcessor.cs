using AMBehaviorSystem.Editor.Utilities;
using AMBehaviorSystem.Node;
using AMBehaviorSystem.Node.SourceGeneration.Context;
using AMBehaviorSystem.Node.SourceGeneration.Traversal;
using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AMBehaviorSystem.Editor.Node
{
    [InitializeOnLoad]
    internal class PipelineGraphProcessor : AssetModificationProcessor
    {
        private const string PendingAssignmentsKey = "AMBS_PendingPipelineAssignments";

        static PipelineGraphProcessor()
        {
            AssemblyReloadEvents.afterAssemblyReload += OnAfterAssemblyReload;
        }

        private static string[] OnWillSaveAssets(string[] paths)
        {
            foreach(string path in paths)
            {
                const int SettingTypeIndex = 0;
                const int ContextTypeIndex = 1;
                const int ProcessorTypeIndex = 2;

                NodeGraph graph = AssetDatabase.LoadAssetAtPath<NodeGraph>(path);

                if(graph == null || graph.TargetController == null)
                    continue;

                Controller controller = (Controller)graph.TargetController;
                Type controllerType = controller.GetType();

                Type settingType;
                Type contextType;
                Type processorType;

                if(GenericUtilities.TryGetInheritedElementTypes(controllerType, out Type[] elementTypes))
                {
                    settingType = elementTypes[SettingTypeIndex];
                    contextType = elementTypes[ContextTypeIndex];
                    processorType = elementTypes[ProcessorTypeIndex];
                }
                else
                    continue;

                string className = $"{graph.name}_Generated";

                SourceContext context = new(className, AMBSSettings.instance.SourceGenerationNamespace, processorType, settingType, contextType);

                GraphTraversal traversal = new(graph, context);
                traversal.Run();

                string fileName = $"{className}.cs";
                string filePath = Path.Combine(Application.dataPath, AMBSSettings.instance.SourceGenerationPath, fileName);

                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                File.WriteAllText(filePath, context.ToString());

                AssetDatabase.Refresh();

                string pipelineFullName = $"{AMBSSettings.instance.SourceGenerationNamespace}.{className}";
                AddPendingAssignment(controller, pipelineFullName);
            }

            return paths;
        }

        private static void AddPendingAssignment(Controller controller, string pipelineFullName)
        {
            string scenePath = controller.gameObject.scene.path;
            string objectPath = GetGameObjectPath(controller.gameObject);

            string entry = $"{scenePath}|{objectPath}|{pipelineFullName}";
            string existing = SessionState.GetString(PendingAssignmentsKey, "");

            SessionState.SetString(PendingAssignmentsKey, string.IsNullOrEmpty(existing) ? entry : $"{existing}\n{entry}");
        }

        private static string GetGameObjectPath(GameObject obj)
        {
            string path = obj.name;
            Transform parent = obj.transform.parent;

            while(parent != null)
            {
                path = $"{parent.name}/{path}";
                parent = parent.parent;
            }

            return path;
        }

        private static void OnAfterAssemblyReload()
        {
            string raw = SessionState.GetString(PendingAssignmentsKey, "");

            if(string.IsNullOrEmpty(raw))
                return;

            SessionState.EraseString(PendingAssignmentsKey);

            foreach(string line in raw.Split('\n'))
            {
                string[] parts = line.Split('|');

                if(parts.Length != 3)
                {
                    Debug.LogError($"[AMBS] Malformed pending assignment entry: '{line}'");
                    continue;
                }

                AssignPipeline(parts[0], parts[1], parts[2]);
            }
        }

        private static void AssignPipeline(string scenePath, string objectPath, string pipelineFullName)
        {
            Type pipelineType = GetType(pipelineFullName);

            if(pipelineType == null)
            {
                Debug.LogError($"[AMBS] Cannot resolve generated pipeline type '{pipelineFullName}'.");
                return;
            }

            Scene scene = SceneManager.GetSceneByPath(scenePath);

            if(!scene.isLoaded)
            {
                Debug.LogWarning($"[AMBS] Scene '{scenePath}' is not currently open. Skipping pipeline assignment; open the scene and re-save the graph to apply it.");
                return;
            }

            GameObject target = FindGameObjectByPath(scene, objectPath);

            if(target == null)
            {
                Debug.LogError($"[AMBS] Cannot find GameObject at path '{objectPath}' in scene '{scenePath}'.");
                return;
            }

            if(!target.TryGetComponent<Controller>(out Controller controller))
            {
                Debug.LogError($"[AMBS] No Controller component on '{target.name}'.");
                return;
            }

            object pipelineInstance = Activator.CreateInstance(pipelineType);

            PropertyInfo pipelineProperty = controller.GetType().GetProperty("Pipeline");

            if(pipelineProperty == null)
            {
                Debug.LogError($"[AMBS] 'Pipeline' property not found on '{controller.GetType()}'.");
                return;
            }

            pipelineProperty.SetValue(controller, pipelineInstance);

            EditorUtility.SetDirty(controller);
            EditorSceneManager.MarkSceneDirty(scene);
        }

        private static Type GetType(string fullName)
        {
            Type type = Type.GetType(fullName);
            if(type != null)
                return type;

            foreach(Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = assembly.GetType(fullName);
                if(type != null)
                    return type;
            }

            return null;
        }

        private static GameObject FindGameObjectByPath(Scene scene, string path)
        {
            string[] segments = path.Split('/');
            GameObject[] roots = scene.GetRootGameObjects();

            GameObject found = Array.Find(roots, r => r.name == segments[0]);
            Transform current = found != null ? found.transform : null;

            if(current == null)
                return null;

            for(int i = 1; i < segments.Length; i++)
            {
                current = current.Find(segments[i]);
                if(current == null)
                    return null;
            }

            return current.gameObject;
        }
    }
}