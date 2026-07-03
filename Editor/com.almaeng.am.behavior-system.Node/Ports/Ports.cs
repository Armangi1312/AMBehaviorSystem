using System;

namespace AMBehaviorSystem.Node.Ports
{
    [Serializable]
    public abstract class Port { }

    [Serializable]
    public class BooleanPort : Port { }

    [Serializable]
    public class NumberPort : Port { }

    [Serializable]
    public class Vector2Port : NumberPort { }

    [Serializable]
    public class Vector3Port : Vector2Port { }

    [Serializable]
    public class Vector4Port : Vector3Port { }

    [Serializable]
    public class PipelineFlowPort : Port { }
}
