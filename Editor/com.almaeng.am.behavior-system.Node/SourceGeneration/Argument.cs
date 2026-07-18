using System;

namespace AMBehaviorSystem.Node.SourceGeneration
{
    public class Argument
    {
        public Type Type { get; }
        public string Term { get; }

        public Argument(Type type, string term)
        {
            Type = type;
            Term = term;
        }

        public override string ToString()
        {
            return Term;
        }
    }
}
