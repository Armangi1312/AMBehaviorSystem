using AMBehaviorSystem.Node.SourceGeneration.Context;

namespace AMBehaviorSystem.Node.SourceGeneration.Traversal
{
    public interface IBranchingSourceGenerationNode : ISourceNode
    {
        void Generate(SourceContext context, GraphTraversal traversal);
    }
}
