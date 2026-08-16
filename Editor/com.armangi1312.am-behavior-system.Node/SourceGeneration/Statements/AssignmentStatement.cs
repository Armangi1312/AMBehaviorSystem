using AMBehaviorSystem.Node.SourceGeneration.Expressions;

namespace AMBehaviorSystem.Node.SourceGeneration.Statements
{
    public class AssignmentStatement : Statement
    {
        public string Name { get; }
        public Expression Value { get; }

        public AssignmentStatement(string name, Expression value)
        {
            Name = name;
            Value = value;
        }

        public override string ToString()
        {
            return $"{Name} = {Value};";
        }
    }
}