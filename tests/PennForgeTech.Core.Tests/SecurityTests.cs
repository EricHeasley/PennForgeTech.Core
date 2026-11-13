using PennForgeTech.Core.Security;
using Xunit;

namespace PennForgeTech.Core.Tests
{
    public sealed class SecurityTests
    {
        [Fact]
        public void ComputeSha256Hex_MatchesKnownHash()
        {
            string hash = HashHelper.ComputeSha256Hex("abc");

            Assert.Equal("BA7816BF8F01CFEA414140DE5DAE2223B00361A396177A9CB410FF61F20015AD", hash);
        }

        [Fact]
        public void PasswordHasher_VerifiesTheCorrectPassword()
        {
            string storedHash = PasswordHasher.Hash("correct horse");

            Assert.True(PasswordHasher.Verify("correct horse", storedHash));
        }

        [Fact]
        public void PasswordHasher_RejectsTheWrongPassword()
        {
            string storedHash = PasswordHasher.Hash("correct horse");

            Assert.False(PasswordHasher.Verify("wrong horse", storedHash));
        }

        [Fact]
        public void PasswordHasher_RejectsMalformedStoredHash()
        {
            Assert.False(PasswordHasher.Verify("anything", "not-a-hash"));
        }

        [Fact]
        public void PasswordHasher_ProducesDifferentHashesForTheSamePassword()
        {
            Assert.NotEqual(PasswordHasher.Hash("same"), PasswordHasher.Hash("same"));
        }

        [Fact]
        public void GenerateToken_ProducesUniqueUrlSafeTokens()
        {
            string firstToken = SecureRandom.GenerateToken();
            string secondToken = SecureRandom.GenerateToken();

            Assert.NotEqual(firstToken, secondToken);
            Assert.DoesNotContain("+", firstToken);
            Assert.DoesNotContain("/", firstToken);
            Assert.DoesNotContain("=", firstToken);
        }

        [Fact]
        public void GetInt_StaysWithinTheRange()
        {
            for (int attempt = 0; attempt < 100; attempt++)
            {
                int number = SecureRandom.GetInt(5, 10);

                Assert.InRange(number, 5, 9);
            }
        }
    }
}
