using System;

namespace AMBehaviorSystem.Node.SourceGeneration
{
    public class DeclarationStatement : Statement
    {
        public Type Type { get; }
        public string Name { get; }
        public Expression Value { get; }

        public DeclarationStatement(Type type, string name, Expression value)
        {
            Type = type;
            Name = name;
            Value = value;
        }

        public override string ToString()
        {
            return $"{TypeUtilities.GetTypeAlias(Type)} {Name} = {Value};";
        }
    }
}