using GraphProcessor;
using System;
using UnityEngine;

namespace AMBehaviorSystem.Node.Data
{
    [Serializable]
    public abstract class BaseDataNode<TOut> : BaseNode
    {
        [Output] public TOut Out;
    }

    [Serializable]
    public abstract class BaseGetControllerDataNode : BaseDataNode<object>
    {
        public string Type;
        public string Path;
    }

    [Serializable]
    [NodeMenuItem("Data/Get Component Data")]
    public class GetComponentDataNode : BaseDataNode<object>
    {
        public override string name => "Get Component Data";

        [SerializeField]
        public SceneObjectReference<GameObject> Target;
        public string Path;
    }

    [Serializable]
    [NodeMenuItem("Data/Get Context Data")]
    public class GetContextDataNode : BaseGetControllerDataNode
    {
        public override string name => "Get Context Data";
    }

    [Serializable]
    [NodeMenuItem("Data/Get Setting Data")]
    public class GetSettingDataNode : BaseGetControllerDataNode
    {
        public override string name => "Get Setting Data";
    }
}
