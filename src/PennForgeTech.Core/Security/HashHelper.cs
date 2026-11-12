using System;
using System.Security.Cryptography;
using System.Text;
using PennForgeTech.Core.Guards;

namespace PennForgeTech.Core.Security
{
    public static class HashHelper
    {
        public static string ComputeSha256Hex(string text)
        {
            Guard.AgainstNull(text, nameof(text));

            byte[] textBytes = Encoding.UTF8.GetBytes(text);
            byte[] hashBytes = SHA256.HashData(textBytes);
            return Convert.ToHexString(hashBytes);
        }
    }
}
