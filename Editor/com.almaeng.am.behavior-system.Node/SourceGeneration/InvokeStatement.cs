namespace AMBehaviorSystem.Node.SourceGeneration
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
