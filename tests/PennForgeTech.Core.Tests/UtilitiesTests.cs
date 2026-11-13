using System.Collections.Generic;
using System.Linq;
using PennForgeTech.Core.Extensions;
using PennForgeTech.Core.Primitives;
using Xunit;

namespace PennForgeTech.Core.Tests
{
    public sealed class UtilitiesTests
    {
        [Fact]
        public void Truncate_ShortensLongText()
        {
            Assert.Equal("Penn", "PennForgeTech".Truncate(4));
        }

        [Fact]
        public void Truncate_LeavesShortTextAlone()
        {
            Assert.Equal("Penn", "Penn".Truncate(10));
        }

        [Fact]
        public void ToSlug_ProducesLowercaseHyphenatedText()
        {
            Assert.Equal("penn-forge-tech-2026", "  Penn Forge, Tech -- 2026! ".ToSlug());
        }

        [Fact]
        public void EqualsIgnoreCase_IgnoresCase()
        {
            Assert.True("Penn".EqualsIgnoreCase("PENN"));
        }

        [Fact]
        public void IsNullOrEmpty_HandlesNullEmptyAndFilledSequences()
        {
            IEnumerable<int>? missing = null;

            Assert.True(missing.IsNullOrEmpty());
            Assert.True(Enumerable.Empty<int>().IsNullOrEmpty());
            Assert.False(new[] { 1 }.IsNullOrEmpty());
        }

        [Fact]
        public void InBatchesOf_SplitsTheSequenceIntoBatches()
        {
            List<IReadOnlyList<int>> batches = Enumerable.Range(1, 5).InBatchesOf(2).ToList();

            Assert.Equal(3, batches.Count);
            Assert.Equal(new[] { 1, 2 }, batches[0]);
            Assert.Equal(new[] { 5 }, batches[2]);
        }

        [Fact]
        public void Base64UrlEncoder_RoundTripsBinaryData()
        {
            byte[] original = { 0xFB, 0xFF, 0xFE, 0x01, 0x02 };

            string encoded = Base64UrlEncoder.Encode(original);
            byte[] decoded = Base64UrlEncoder.Decode(encoded);

            Assert.Equal(original, decoded);
            Assert.DoesNotContain("=", encoded);
        }
    }
}
