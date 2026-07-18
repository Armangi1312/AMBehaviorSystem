using GraphProcessor;
using System;
using System.Collections.Generic;

namespace AMBehaviorSystem.Node.SourceGeneration
{
    internal static class NodeUtilities
    {
        public static (Type Type, string Name) GetInputVariable(string fieldName, SourceContext context, BaseNode node)
        {
            NodePort port = node.inputPorts.Find(p => p.fieldName == fieldName);
            List<SerializableEdge> edges = port.GetEdges();

            if (edges.Count == 0)
                throw new InvalidOperationException($"Port {fieldName} on {node.name} has no connected edge.");

            BaseNode sourceNode = edges[0].outputNode;

            if (context.OutputLocals.TryGetValue(sourceNode.GUID, out (Type Type, string Name) local))
                return local;

            if (context.OutputFields.TryGetValue(sourceNode.GUID, out (Type Type, string Name) field))
                return field;

            throw new InvalidOperationException($"No output variable found for node {sourceNode.name} ({sourceNode.GUID}).");
        }

    }
}