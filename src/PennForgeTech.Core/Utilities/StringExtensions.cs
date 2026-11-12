using System;
using System.Text;
using PennForgeTech.Core.Guards;

namespace PennForgeTech.Core.Utilities
{
    public static class StringExtensions
    {
        public static string Truncate(this string text, int maximumLength)
        {
            Guard.AgainstNull(text, nameof(text));
            Guard.AgainstBelowMinimum(maximumLength, 0, nameof(maximumLength));

            if (text.Length <= maximumLength)
            {
                return text;
            }

            return text.Substring(0, maximumLength);
        }

        public static bool EqualsIgnoreCase(this string text, string other)
        {
            return string.Equals(text, other, StringComparison.OrdinalIgnoreCase);
        }

        public static string ToSlug(this string text)
        {
            Guard.AgainstNull(text, nameof(text));

            StringBuilder slug = new StringBuilder();
            bool lastCharacterWasSeparator = true;

            foreach (char character in text)
            {
                if (char.IsAsciiLetterOrDigit(character))
                {
                    slug.Append(char.ToLowerInvariant(character));
                    lastCharacterWasSeparator = false;
                }
                else if (!lastCharacterWasSeparator)
                {
                    slug.Append('-');
                    lastCharacterWasSeparator = true;
                }
            }

            return slug.ToString().TrimEnd('-');
        }
    }
}
