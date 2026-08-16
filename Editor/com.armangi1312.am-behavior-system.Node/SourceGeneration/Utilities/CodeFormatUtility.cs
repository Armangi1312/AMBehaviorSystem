using AMBehaviorSystem.Node.SourceGeneration.Statements;
using System.Collections.Generic;
using System.Text;

namespace AMBehaviorSystem.Node.SourceGeneration.Utilities
{
    internal static class CodeFormatUtility
    {
        public static string Indent(string text, int level = 1)
        {
            string prefix = new('\t', level);
            string[] lines = text.Split('\n');
            StringBuilder builder = new();

            for (int i = 0; i < lines.Length; i++)
            {
                if (i > 0) builder.Append('\n');
                builder.Append(prefix).Append(lines[i]);
            }

            return builder.ToString();
        }

        public static string RenderStatements(IReadOnlyList<Statement> statements, int indentLevel)
        {
            StringBuilder builder = new();

            foreach (Statement statement in statements)
            {
                builder.AppendLine(Indent(statement.ToString(), indentLevel));
            }

            return builder.ToString();
        }

        public static string ToCamelCase(string text)
        {
            if (string.IsNullOrEmpty(text) || char.IsLower(text[0]))
                return text;

            char[] chars = text.ToCharArray();

            for (int i = 0; i < chars.Length; i++)
            {
                bool hasNext = i + 1 < chars.Length;

                if (i > 0 && hasNext && !char.IsUpper(chars[i + 1]))
                    break;

                chars[i] = char.ToLowerInvariant(chars[i]);
            }

            return new string(chars);
        }
    }
}