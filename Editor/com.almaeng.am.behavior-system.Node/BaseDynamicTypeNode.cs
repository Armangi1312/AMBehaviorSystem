using GraphProcessor;
using System;
using System.Collections.Generic;

namespace AMBehaviorSystem.Node
{
    public abstract class BaseDynamicTypeNode : BaseNode
    {
        public Type CurrentType { get; protected set; }
        protected abstract Type DefaultType { get; }

        protected IEnumerable<PortData> CreatePort(string portName, bool multiple)
        {
            yield return new PortData
            {
                identifier = portName,
                displayName = portName,
                displayType = CurrentType,
                acceptMultipleEdges = multiple
            };
        }

        protected override void Enable()
        {
            if (CurrentType == null)
                CurrentType = DefaultType;

            onAfterEdgeConnected -= HandleEdgeConnected;
            onAfterEdgeConnected += HandleEdgeConnected;

            onAfterEdgeDisconnected -= HandleEdgeDisconnected;
            onAfterEdgeDisconnected += HandleEdgeDisconnected;
        }

        protected virtual void HandleEdgeConnected(SerializableEdge edge)
        {
            if (edge.inputNode != this)
                return;

            Type newType = edge.outputPort.portData.displayType;

            if (newType == CurrentType)
                return;

            CurrentType = newType;
            UpdateAllPorts();
        }

        protected virtual void HandleEdgeDisconnected(SerializableEdge edge)
        {
            foreach (NodePort port in inputPorts)
            {
                List<SerializableEdge> edges = port.GetEdges();

                if (edges.Count > 0)
                {
                    CurrentType = edges[0].outputPort.portData.displayType;
                    UpdateAllPorts();
                    return;
                }
            }

            CurrentType = DefaultType;
            UpdateAllPorts();
        }
    }
}