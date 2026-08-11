using AMBehaviorSystem.Node.SourceGeneration.Expressions;
using System.Collections.Generic;
using System.Text;

namespace AMBehaviorSystem.Node.SourceGeneration.Statements
{
    public class IfStatement : Statement
    {
        public Expression Condition { get; }
        public List<Statement> TrueStatements { get; }
        public List<Statement> FalseStatements { get; }

        public IfStatement(Expression condition, List<Statement> trueStatements, List<Statement> falseStatements)
        {
            Condition = condition;
            TrueStatements = trueStatements;
            FalseStatements = falseStatements;
        }

        public override string ToString()
        {
            StringBuilder builder = new();

            builder.AppendLine($"if ({Condition})");
            builder.AppendLine("{");

            foreach (Statement statement in TrueStatements)
            {
                builder.AppendLine($"\t{statement}");
            }

            builder.AppendLine("}");
            builder.AppendLine("else");
            builder.AppendLine("{");

            foreach (Statement statement in FalseStatements)
            {
                builder.AppendLine($"\t{statement}");
            }

            builder.Append("}");

            return builder.ToString();
        }
    }
}