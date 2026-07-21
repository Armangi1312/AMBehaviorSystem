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

            SerializableEdge edge = edges[0];
            BaseNode sourceNode = edge.outputNode;

            string portIdentifier = string.IsNullOrEmpty(edge.outputPortIdentifier)
                ? edge.outputFieldName
                : edge.outputPortIdentifier;

            PortKey key = string.IsNullOrEmpty(portIdentifier)
                ? PortKey.Default(sourceNode.GUID)
                : PortKey.Of(sourceNode.GUID, portIdentifier);

            if (context.OutputLocals.TryGetValue(key, out (Type Type, string Name) local))
                return local;

            if (context.OutputFields.TryGetValue(key, out (Type Type, string Name) field))
                return field;

            throw new InvalidOperationException($"No output variable found for node {sourceNode.name} ({sourceNode.GUID}), port {portIdentifier}.");
        }
    }
}