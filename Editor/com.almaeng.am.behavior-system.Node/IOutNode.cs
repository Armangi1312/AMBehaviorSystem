using AMBehaviorSystem.Node.Ports;

namespace AMBehaviorSystem.Node.Math
{
    public interface IOutNode<TOutput> : INode
        where TOutput : Port
    {
        TOutput Out { get; set; }
    }
}
