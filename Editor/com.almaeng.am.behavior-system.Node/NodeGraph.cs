using AMBehaviorSystem.Node.Pipelines;
using GraphProcessor;
using UnityEngine;

namespace AMBehaviorSystem.Node
{
    [CreateAssetMenu(menuName = "Pipelines Graph")]
    public class NodeGraph : BaseGraph
    {
        [SerializeField] private SceneObjectReference<Object> controllerReference = new();

        public Object TargetController
        {
            get => controllerReference.Value;
            set => controllerReference.Value = value;
        }

        public void Resolve() => controllerReference.Resolve();
    }
}
