using AMBehaviorSystem.Node.SourceGeneration;
using GraphProcessor;
using UnityEngine;

namespace AMBehaviorSystem.Node
{
    [CreateAssetMenu(menuName = "Pipelines Graph")]
    public class NodeGraph : BaseGraph
    {
        [SerializeField] private SceneObjectReference<MonoBehaviour> controllerReference = new();

        public MonoBehaviour TargetController
        {
            get => controllerReference.Value;
            set => controllerReference.Value = value;
        }

        public SourceContext SourceContext { get; } = new("Test", "Test", typeof(int), typeof(int), typeof(int));

        public void Resolve() => controllerReference.Resolve();
    }
}
