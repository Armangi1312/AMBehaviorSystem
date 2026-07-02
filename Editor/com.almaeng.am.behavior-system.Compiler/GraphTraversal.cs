using AMBehaviorSystem.Node;
using GraphProcessor;

namespace AMBehaviorSystem.Compiler
{
    internal static class GraphTraversal
    {
        public static BaseNode GetOutputNode(NodeGraph graph, BaseNode fromNode, string portFieldName)
        {
            foreach (SerializableEdge edge in graph.edges)
            {
                if (edge.outputNode != fromNode) continue;
                if (edge.outputFieldName != portFieldName) continue;
                return edge.inputNode;
            }

            return null;
        }

        public static BaseNode GetInputNode(NodeGraph graph, BaseNode toNode, string portFieldName)
        {
            foreach (SerializableEdge edge in graph.edges)
            {
                if (edge.inputNode != toNode) continue;
                if (edge.inputFieldName != portFieldName) continue;
                return edge.outputNode;
            }

            return null;
        }

        public static (BaseNode Node, string PortName) GetInputNodeWithPort(NodeGraph graph, BaseNode toNode, string portFieldName)
        {
            foreach (SerializableEdge edge in graph.edges)
            {
                if (edge.inputNode != toNode) continue;
                if (edge.inputFieldName != portFieldName) continue;
                return (edge.outputNode, edge.outputFieldName);
            }

            return (null, null);
        }
    }
}