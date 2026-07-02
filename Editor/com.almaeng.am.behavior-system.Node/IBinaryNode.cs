using AMBehaviorSystem.Node.Ports;

namespace AMBehaviorSystem.Node.Math
{
    public interface IBinaryNode<TInput> : INode
        where TInput : Port
    {
        TInput A { get; set; }
        TInput B { get; set; }
    }
}
