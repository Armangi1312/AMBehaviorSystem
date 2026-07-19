using System;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AMBehaviorSystem.Node.SourceGeneration
{
    public class Expression
    {
        public Argument[] Arguments { get; }
        public ExpressionRule[] ExpressionRules { get; }

        public Expression(Argument argument, params ExpressionRule[] expressionRules)
        {
            Arguments = new[] { argument };
            ExpressionRules = expressionRules;
        }

        public Expression(Argument[] arguments, params ExpressionRule[] expressionRules)
        {
            Arguments = arguments;
            ExpressionRules = expressionRules;
        }

        public override string ToString()
        {
            if (Arguments == null || Arguments.Length == 0 || ExpressionRules == null || ExpressionRules.Length == 0)
                return string.Empty;

            ExpressionRule rule = ExpressionRules.FirstOrDefault(r => r.Match(Arguments)) ?? throw new InvalidOperationException("No matching ExpressionRule for given Arguments.");
            Type[] constrainedTypes = rule.GetConstraintedTypes(Arguments);

            StringBuilder builder = new();
            int index = 0;

            for (int i = 0; i < rule.Template.Length; i++)
            {
                if (rule.Template[i] == '#' && index < Arguments.Length)
                {
                    Argument argument = Arguments[index];
                    Type constrainedType = constrainedTypes[index];

                    if (argument.Type == constrainedType)
                        builder.Append(argument.ToString());
                    else
                        builder.Append(argument.Type.GetCastedExpression(constrainedType, argument.ToString()));

                    index++;
                    continue;
                }

                builder.Append(rule.Template[i]);
            }

            return builder.ToString();
        }
    }
}