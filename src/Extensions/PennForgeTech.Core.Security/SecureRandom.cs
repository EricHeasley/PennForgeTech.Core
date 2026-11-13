using System.Security.Cryptography;
using PennForgeTech.Core.Primitives;

namespace PennForgeTech.Core.Security
{
    public static class SecureRandom
    {
        public static byte[] GetBytes(int byteCount)
        {
            Guard.AgainstBelowMinimum(byteCount, 1, nameof(byteCount));
            return RandomNumberGenerator.GetBytes(byteCount);
        }

        public static int GetInt(int minimumInclusive, int maximumExclusive)
        {
            return RandomNumberGenerator.GetInt32(minimumInclusive, maximumExclusive);
        }

        public static string GenerateToken(int byteCount = 32)
        {
            return Base64UrlEncoder.Encode(GetBytes(byteCount));
        }
    }
}
