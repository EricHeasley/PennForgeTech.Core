using System;
using System.Security.Cryptography;
using System.Text;
using PennForgeTech.Core.Primitives;

namespace PennForgeTech.Core.Security
{
    public static class PasswordHasher
    {
        private const int SaltByteCount = 16;
        private const int HashByteCount = 32;
        private const int IterationCount = 310000;
        private const char PartSeparator = '.';

        public static string Hash(string password)
        {
            Guard.AgainstNullOrWhiteSpace(password, nameof(password));

            byte[] salt = SecureRandom.GetBytes(SaltByteCount);
            byte[] hash = ComputeHash(password, salt, IterationCount);

            return string.Join(
                PartSeparator,
                IterationCount,
                Convert.ToBase64String(salt),
                Convert.ToBase64String(hash));
        }

        public static bool Verify(string password, string storedHash)
        {
            Guard.AgainstNullOrWhiteSpace(password, nameof(password));
            Guard.AgainstNullOrWhiteSpace(storedHash, nameof(storedHash));

            string[] parts = storedHash.Split(PartSeparator);

            if (parts.Length != 3 || !int.TryParse(parts[0], out int iterationCount) || iterationCount < 1)
            {
                return false;
            }

            try
            {
                byte[] salt = Convert.FromBase64String(parts[1]);
                byte[] expectedHash = Convert.FromBase64String(parts[2]);
                byte[] actualHash = ComputeHash(password, salt, iterationCount);

                return CryptographicOperations.FixedTimeEquals(expectedHash, actualHash);
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private static byte[] ComputeHash(string password, byte[] salt, int iterationCount)
        {
            return Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                iterationCount,
                HashAlgorithmName.SHA256,
                HashByteCount);
        }
    }
}
