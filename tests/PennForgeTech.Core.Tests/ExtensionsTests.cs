using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using PennForgeTech.Core.Extensions;
using PennForgeTech.Core.Results;
using Xunit;

namespace PennForgeTech.Core.Tests
{
    public sealed class ExtensionsTests
    {
        private enum Status
        {
            [Description("Waiting for approval")]
            Pending,
            Approved
        }

        [Fact]
        public void ContainsIgnoreCase_IgnoresCase()
        {
            Assert.True("PennForgeTech".ContainsIgnoreCase("forge"));
        }

        [Fact]
        public void RemoveWhitespace_RemovesSpacesAndTabs()
        {
            Assert.Equal("PennForge", " Penn \tForge ".RemoveWhitespace());
        }

        [Theory]
        [InlineData("eric@example.com", true)]
        [InlineData("not-an-email", false)]
        [InlineData("Eric <eric@example.com>", false)]
        [InlineData("", false)]
        public void IsValidEmailAddress_ChecksTheFormat(string text, bool expected)
        {
            Assert.Equal(expected, text.IsValidEmailAddress());
        }

        [Fact]
        public void WhereNotNull_SkipsNullItems()
        {
            List<string?> items = new List<string?> { "a", null, "b" };

            Assert.Equal(new[] { "a", "b" }, items.WhereNotNull().ToList());
        }

        [Fact]
        public void ForEach_RunsTheActionForEachItem()
        {
            List<int> seen = new List<int>();

            new[] { 1, 2, 3 }.ForEach(number => seen.Add(number));

            Assert.Equal(new[] { 1, 2, 3 }, seen);
        }

        [Fact]
        public void GetOrAdd_AddsOnlyWhenTheKeyIsMissing()
        {
            Dictionary<string, int> scores = new Dictionary<string, int> { ["eric"] = 5 };

            int existing = scores.GetOrAdd("eric", () => 99);
            int added = scores.GetOrAdd("sam", () => 7);

            Assert.Equal(5, existing);
            Assert.Equal(7, added);
            Assert.Equal(7, scores["sam"]);
        }

        [Fact]
        public void StartOfDay_AndEndOfDay_BoundTheDay()
        {
            DateTimeOffset value = new DateTimeOffset(2026, 3, 15, 13, 45, 0, TimeSpan.FromHours(-5));

            Assert.Equal(new DateTimeOffset(2026, 3, 15, 0, 0, 0, TimeSpan.FromHours(-5)), value.StartOfDay());
            Assert.Equal(new DateTimeOffset(2026, 3, 16, 0, 0, 0, TimeSpan.FromHours(-5)).AddTicks(-1), value.EndOfDay());
        }

        [Fact]
        public void StartOfMonth_ReturnsTheFirstDay()
        {
            DateTimeOffset value = new DateTimeOffset(2026, 3, 15, 13, 45, 0, TimeSpan.Zero);

            Assert.Equal(new DateTimeOffset(2026, 3, 1, 0, 0, 0, TimeSpan.Zero), value.StartOfMonth());
        }

        [Fact]
        public void IsWeekend_RecognizesSaturdayAndSunday()
        {
            Assert.True(new DateTimeOffset(2026, 3, 14, 0, 0, 0, TimeSpan.Zero).IsWeekend());
            Assert.False(new DateTimeOffset(2026, 3, 16, 0, 0, 0, TimeSpan.Zero).IsWeekend());
        }

        [Fact]
        public void GetDescription_UsesTheAttributeOrFallsBackToTheName()
        {
            Assert.Equal("Waiting for approval", Status.Pending.GetDescription());
            Assert.Equal("Approved", Status.Approved.GetDescription());
        }

        [Fact]
        public void ToResult_FailsForNullAndSucceedsOtherwise()
        {
            ResultError error = new ResultError("Item.Missing", "No item.");
            string? missing = null;

            Assert.True(missing.ToResult(error).IsFailure);
            Assert.Equal("found", "found".ToResult(error).Value);
        }
    }
}
