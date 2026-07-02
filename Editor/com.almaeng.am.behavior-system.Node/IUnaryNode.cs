using AMBehaviorSystem.Node.Ports;

namespace AMBehaviorSystem.Node.Math
{
    public interface IUnaryNode<TInput> : INode
        where TInput : Port
    {
        TInput Input { get; set; }
    }
}
