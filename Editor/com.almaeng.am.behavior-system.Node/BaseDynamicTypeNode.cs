using GraphProcessor;
using System;
using System.Collections.Generic;

namespace AMBehaviorSystem.Node
{
    [Serializable]
    public abstract class BaseDynamicTypeNode : BaseNode
    {
        protected Type CurrentType;
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

       [CustomPortTypeBehavior(typeof(Ports.Port))]
       private IEnumerable<PortData> DynamicPortBehavior(string fieldName, string displayName, object value)
       {
           yield return new PortData
           {
               identifier = fieldName,
               displayName = displayName,
               displayType = CurrentType,
               acceptMultipleEdges = false
           };
       }

        protected override void Enable()
        {
            CurrentType = DefaultType;

            onAfterEdgeConnected -= HandleEdgeConnected;
            onAfterEdgeConnected += HandleEdgeConnected;

            onAfterEdgeDisconnected -= HandleEdgeDisconnected;
            onAfterEdgeDisconnected += HandleEdgeDisconnected;
        }

        protected virtual void HandleEdgeConnected(SerializableEdge edge)
        {
            if (edge.inputNode != this) return;

            Type newType = edge.outputPort.portData.displayType;

            if (newType == CurrentType || !CurrentType.IsAssignableFrom(newType)) return;

            CurrentType = newType;
            UpdateAllPorts();
        }

        protected virtual void HandleEdgeDisconnected(SerializableEdge edge)
        {
            if (edge.inputNode != this) return;

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
