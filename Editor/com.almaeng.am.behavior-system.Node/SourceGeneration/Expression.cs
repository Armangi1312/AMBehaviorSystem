using System;
using System.Text;
using System.Linq;

namespace AMBehaviorSystem.Node.SourceGeneration
{
    public class Expression
    {
        public Argument[] Arguments { get; }
        public ArgumentRule[] ArgumentRules { get; }

        public Type[] ExpectedTypes { get; }
        public Type Out { get; }

        public string Template { get; }

        public Expression(Argument[] arguments, Type[] expectedTypes, Type @out, string template)
        {
            Arguments = arguments;
            ExpectedTypes = expectedTypes;
            Out = @out;
            Template = template;
        }

        public Expression(Argument[] arguments, ArgumentRule[] argumentRules, Type[] expectedTypes, Type @out, string template)
        {
            Arguments = arguments;
            ArgumentRules = argumentRules;
            ExpectedTypes = expectedTypes;
            Out = @out;
            Template = template;
        }

        public override string ToString()
        {
            if (Arguments == null || Arguments.Length == 0)
                return string.Empty;

            ArgumentRule rule = ArgumentRules?.FirstOrDefault(rule => rule.IsMatched());

            string template = rule == null ? Template : rule.Template;
            Type[] expectedTypes = rule == null ? ExpectedTypes : rule.ExpectedTypes;

            if (expectedTypes.Length < Arguments.Length)
                throw new InvalidOperationException("ExpectedTypes length is smaller than Arguments length.");

            StringBuilder builder = new();
            int index = 0;

            for (int i = 0; i < template.Length; i++)
            {
                if (template[i] == '#' && index < Arguments.Length)
                {
                    Argument argument = Arguments[index];
                    Type expectedType = expectedTypes[index];

                    bool isExactMatch = expectedType == typeof(object) || expectedType.IsAssignableFrom(argument.Type);
                    bool isNumericFilterMatch = expectedType == typeof(NumericFilter) && argument.Type.IsNumeric();
                    bool isVectorFilterMatch = expectedType == typeof(VectorFilter) && argument.Type.IsVector();
                    bool isScalarFilterMatch = expectedType == typeof(ScalarFilter) && argument.Type.IsScalar();

                    bool isExpectedType = isExactMatch || isNumericFilterMatch || isVectorFilterMatch || isScalarFilterMatch;

                    if (isExpectedType)
                        builder.Append(argument.ToString());
                    else
                        builder.Append(TypeUtilities.GetCastedExpression(argument.Type, expectedType, argument.Term));

                    index++;
                    continue;
                }

                builder.Append(template[i]);
            }

            return builder.ToString();
        }
    }
}