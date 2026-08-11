using AMBehaviorSystem.Node.SourceGeneration.Expressions;

namespace AMBehaviorSystem.Node.SourceGeneration.Statements
{
    public class InvokeStatement : Statement
    {
        public Expression Method { get; }

        public InvokeStatement(Expression method)
        {
            Method = method;
        }

        public override string ToString()
        {
            return $"{Method};";
        }
    }
}
