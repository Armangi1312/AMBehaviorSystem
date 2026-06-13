using AMBehaviorSystem.Node;
using GraphProcessor;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace AMBehaviorSystem.Editor.Node
{
    public class NodeGraphWindow : BaseGraphWindow
    {
        [OnOpenAsset(0)]
        public static bool OnGraphOpened(EntityId entityId, int line)
        {
            NodeGraph asset = EditorUtility.EntityIdToObject(entityId) as NodeGraph;

            if (asset == null) return false;

            GetWindow<NodeGraphWindow>().InitializeGraph(asset);
            return true;
        }

        protected override void InitializeWindow(BaseGraph graph)
        {
            titleContent = new GUIContent("Pipelines Graph");

            graphView ??= new NodeGraphView(this);

            rootView.Add(graphView);
        }
    }
}