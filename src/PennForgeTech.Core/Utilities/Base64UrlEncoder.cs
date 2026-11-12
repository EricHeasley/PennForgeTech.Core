using System;
using PennForgeTech.Core.Guards;

namespace PennForgeTech.Core.Utilities
{
    public static class Base64UrlEncoder
    {
        public static string Encode(byte[] data)
        {
            Guard.AgainstNull(data, nameof(data));

            return Convert.ToBase64String(data)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
        }

        public static byte[] Decode(string text)
        {
            Guard.AgainstNull(text, nameof(text));

            string base64Text = text.Replace('-', '+').Replace('_', '/');
            int missingPaddingLength = (4 - (base64Text.Length % 4)) % 4;
            base64Text = base64Text + new string('=', missingPaddingLength);

            return Convert.FromBase64String(base64Text);
        }
    }
}
