using AMBehaviorSystem.Editor.Node;
using AMBehaviorSystem.Editor.Utilities;
using AMBehaviorSystem.Node;
using System;
using UnityEditor;
using UnityEngine.UIElements;

namespace AMBehaviorSystem.Editor.Inspectors
{
    [CustomEditor(typeof(Controller), true)]
    internal class ControllerEditor : UnityEditor.Editor
    {
        private const int SettingTypeIndex = 0;
        private const int ContextTypeIndex = 1;
        private const int ProcessorTypeIndex = 2;

        private const string PipelineGraphHelpText = "If 'Pipeline Graph' is null, processors run in the order they were registered.\nTo configure a custom execution order, use the Pipeline Graph.";

        private Controller controller;

        private SerializedProperty settingsProperty;
        private SerializedProperty contextsProperty;
        private SerializedProperty processorsProperty;

        private SerializedProperty pipelineProperty;
        private SerializedProperty pipelineGraphProperty;

        private VisualElement root;

        private void OnEnable()
        {
            controller = target as Controller;

            settingsProperty = serializedObject.FindProperty("<Settings>k__BackingField");
            contextsProperty = serializedObject.FindProperty("<Contexts>k__BackingField");
            processorsProperty = serializedObject.FindProperty("<Processors>k__BackingField")
                                                 .FindPropertyRelative("Items");

            pipelineProperty = serializedObject.FindProperty("<Pipeline>k__BackingField");
            pipelineGraphProperty = serializedObject.FindProperty("pipelineGraph");
        }

        public override VisualElement CreateInspectorGUI()
        {
            root = new();

            AddRegistryField(contextsProperty, ContextTypeIndex, controller.IsContextRequired);
            AddRegistryField(settingsProperty, SettingTypeIndex, controller.IsSettingRequired);
            AddProcessorListField();
            AddPipelineGraphField();

            return root;
        }

        private void AddRegistryField(SerializedProperty property, int typeIndex, Func<Type, bool> removalValidator)
        {
            if(property == null)
                return;
            if(!GenericUtilities.TryGetInheritedGenericType(controller.GetType(), typeIndex, out Type elementType))
                return;

            RegistryField field = new(property, property.FindPropertyRelative("<SerializedObjects>k__BackingField"), elementType, removalValidator);
            field.style.marginTop = 8;
            root.Add(field);
        }

        private void AddProcessorListField()
        {
            if(processorsProperty == null)
                return;
            if(!GenericUtilities.TryGetInheritedGenericType(controller.GetType(), ProcessorTypeIndex, out Type processorType))
                return;

            ProcessorListField field = new(processorsProperty, processorType, controller);
            field.style.marginTop = 8;
            root.Add(field);
        }

        private void AddPipelineGraphField()
        {
            if(pipelineProperty == null || pipelineGraphProperty == null)
                return;

            if(pipelineGraphProperty.objectReferenceValue == null)
            {
                root.Add(new HelpBox(PipelineGraphHelpText, HelpBoxMessageType.Info));
                root.Add(CreatePipelineGraphButton());
            }
            else
            {
                root.Add(CreateOpenPipelineGraphButton());
            }
        }

        private Button CreatePipelineGraphButton()
        {
            Button button = new() { text = "Create or Select Pipeline Graph" };
            button.clicked += OnCreatePipelineGraphClicked;
            return button;
        }

        private void OnCreatePipelineGraphClicked()
        {
            string path = EditorUtility.SaveFilePanelInProject(
                "Create Pipeline Graph",
                controller.GetType().Name,
                "asset",
                "Select a location to save the Pipeline Graph asset."
            );

            if(string.IsNullOrEmpty(path))
                return;

            NodeGraph pipelineGraph = AssetDatabase.LoadAssetAtPath<NodeGraph>(path);

            if(pipelineGraph == null)
            {
                pipelineGraph = CreateInstance<NodeGraph>();
                pipelineGraph.TargetController = controller;
                pipelineGraph.Resolve();

                AssetDatabase.CreateAsset(pipelineGraph, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            pipelineGraphProperty.objectReferenceValue = pipelineGraph;
            serializedObject.ApplyModifiedProperties();

            root.Clear();
            root.Add(CreateInspectorGUI());
        }

        private Button CreateOpenPipelineGraphButton()
        {
            Button button = new() { text = "Open Pipeline Graph" };
            button.clicked += OnOpenPipelineGraphClicked;
            return button;
        }

        private void OnOpenPipelineGraphClicked()
        {
            if(pipelineGraphProperty.objectReferenceValue is not NodeGraph pipelineGraph)
                return;

            NodeGraphWindow.OnGraphOpened(pipelineGraph.GetEntityId());
        }
    }
}