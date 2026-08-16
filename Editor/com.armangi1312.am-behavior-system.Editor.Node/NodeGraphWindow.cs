using AMBehaviorSystem.Node;
using AMBehaviorSystem.Node.Pipelines;
using GraphProcessor;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace AMBehaviorSystem.Editor.Node
{
    public class NodeGraphWindow : BaseGraphWindow
    {
        [OnOpenAsset(0)]
        public static bool OnGraphOpened(EntityId entityId)
        {
            NodeGraph asset = EditorUtility.EntityIdToObject(entityId) as NodeGraph;

            if(asset == null)
                return false;

            bool hasEntryNode = asset.nodes.Any(node => node is EntryNode);

            if(!hasEntryNode)
            {
                EntryNode entryNode = new();
                entryNode.OnNodeCreated();
                asset.AddNode(entryNode);
            }

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
