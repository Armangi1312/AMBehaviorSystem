using System;
using System.Collections.Generic;
using System.Text;

namespace AMBehaviorSystem.Node.SourceGeneration
{
    internal static class StringUtilities
    {
        public static string ToCamelCase(this string text)
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
