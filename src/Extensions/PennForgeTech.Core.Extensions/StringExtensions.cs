using System;
using System.Linq;
using System.Net.Mail;
using System.Text;
using PennForgeTech.Core.Primitives;

namespace PennForgeTech.Core.Extensions
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

        public static bool ContainsIgnoreCase(this string text, string value)
        {
            Guard.AgainstNull(text, nameof(text));
            Guard.AgainstNull(value, nameof(value));

            return text.Contains(value, StringComparison.OrdinalIgnoreCase);
        }

        public static string RemoveWhitespace(this string text)
        {
            Guard.AgainstNull(text, nameof(text));

            return new string(text.Where(character => !char.IsWhiteSpace(character)).ToArray());
        }

        public static bool IsValidEmailAddress(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return false;
            }

            if (!MailAddress.TryCreate(text, out MailAddress? address))
            {
                return false;
            }

            return string.Equals(address.Address, text.Trim(), StringComparison.Ordinal);
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
