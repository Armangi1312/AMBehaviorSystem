using AMBehaviorSystem.Node.SourceGeneration.Context;

namespace AMBehaviorSystem.Node.SourceGeneration.Traversal
{
    public interface ISourceGenerationNode
    {
        void Generate(SourceContext context);
    }
}
