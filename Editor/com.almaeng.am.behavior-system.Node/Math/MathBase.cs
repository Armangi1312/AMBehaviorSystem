using AMBehaviorSystem.Node.Ports;
using GraphProcessor;
using System;
using System.Collections.Generic;

namespace AMBehaviorSystem.Node.Math
{
    [Serializable]
    public abstract class BaseMathNode : BaseNode { }

    [Serializable]
    public abstract class BaseMathNode<TOut> : BaseMathNode
        where TOut : Port
    {
        [Output] public TOut Out;
    }

    [Serializable]
    public abstract class BaseChangeableMathNode<TOut> : BaseMathNode<TOut>
        where TOut : Port
    {
        protected Type currentType = typeof(NumberPort);
        protected virtual Type defaultType => typeof(NumberPort);

        protected IEnumerable<PortData> CreatePort(string portName, bool multiple)
        {
            yield return new PortData
            {
                identifier = portName,
                displayName = portName,
                displayType = currentType,
                acceptMultipleEdges = multiple
            };
        }

        protected override void Enable()
        {
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

            if (newType == currentType || !currentType.IsAssignableFrom(newType))
                return;

            currentType = newType;
            UpdateAllPorts();
        }

        protected virtual void HandleEdgeDisconnected(SerializableEdge edge)
        {
            foreach (var port in inputPorts)
            {
                var edges = port.GetEdges();

                if (edges.Count > 0)
                {
                    currentType = edges[0].outputPort.portData.displayType;
                    UpdateAllPorts();
                    return;
                }
            }

            currentType = defaultType;
            UpdateAllPorts();
        }
    }

    [Serializable]
    public abstract class BaseChangeableMathNode : BaseChangeableMathNode<Port>
    {
        [CustomPortBehavior(nameof(Out))]
        public IEnumerable<PortData> OutPort(List<SerializableEdge> _) => CreatePort(nameof(Out), true);
    }

    [Serializable]
    public abstract class BaseSplitableMathNode : BaseMathNode
    {
        protected Type currentType = typeof(Vector2Port);
        protected virtual Type defaultType => typeof(Vector2Port);

        protected IEnumerable<PortData> CreatePort(string portName, bool multiple)
        {
            yield return new PortData
            {
                identifier = portName,
                displayName = portName,
                displayType = currentType,
                acceptMultipleEdges = multiple
            };
        }

        protected override void Enable()
        {
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

            if (newType == currentType || !currentType.IsAssignableFrom(newType))
                return;

            currentType = newType;
            UpdateAllPorts();
        }

        protected virtual void HandleEdgeDisconnected(SerializableEdge edge)
        {
            foreach (var port in inputPorts)
            {
                var edges = port.GetEdges();

                if (edges.Count > 0)
                {
                    currentType = edges[0].outputPort.portData.displayType;
                    UpdateAllPorts();
                    return;
                }
            }

            currentType = defaultType;
            UpdateAllPorts();
        }
    }
}
