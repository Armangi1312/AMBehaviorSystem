namespace AMBehaviorSystem.Node
{
    public interface IFieldNode<TField> : INode
        where TField : struct
    {
        TField Field { get; }
    }
}
