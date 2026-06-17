using GraphProcessor;
using System;
using UnityEngine;

namespace AMBehaviorSystem.Node.Data
{
    [Serializable]
    [NodeMenuItem("Data/GetComponentData")]
    public class GetComponentDataNode : BaseDataNode<object>
    {
        public override string name => "Get Component Data";

        [SerializeField]
        public SceneObjectReference<UnityEngine.Object> Target;
        public string Path;
    }
}
