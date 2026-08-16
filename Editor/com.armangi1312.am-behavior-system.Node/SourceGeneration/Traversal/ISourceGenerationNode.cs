using AMBehaviorSystem.Node.SourceGeneration.Context;

namespace AMBehaviorSystem.Node.SourceGeneration.Traversal
{
    public interface ISourceGenerationNode : ISourceNode
    {
        void Generate(SourceContext context);
    }
}
