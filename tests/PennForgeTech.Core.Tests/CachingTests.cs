using System;
using PennForgeTech.Core.Caching;
using PennForgeTech.Core.Time;
using Xunit;

namespace PennForgeTech.Core.Tests
{
    public sealed class CachingTests
    {
        private readonly FixedClock _clock = new FixedClock(new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero));

        [Fact]
        public void TryGet_ReturnsStoredValueBeforeItExpires()
        {
            MemoryCacheService cache = new MemoryCacheService(this._clock);
            cache.Store("name", "Eric", TimeSpan.FromMinutes(5));

            bool found = cache.TryGet("name", out string? value);

            Assert.True(found);
            Assert.Equal("Eric", value);
        }

        [Fact]
        public void TryGet_ReturnsFalseAfterTheValueExpires()
        {
            MemoryCacheService cache = new MemoryCacheService(this._clock);
            cache.Store("name", "Eric", TimeSpan.FromMinutes(5));

            this._clock.Advance(TimeSpan.FromMinutes(6));

            Assert.False(cache.TryGet("name", out string? _));
        }

        [Fact]
        public void GetOrCreate_CreatesTheValueOnlyOnce()
        {
            MemoryCacheService cache = new MemoryCacheService(this._clock);
            int createCount = 0;

            Func<string> create = () =>
            {
                createCount++;
                return "created";
            };

            cache.GetOrCreate("key", TimeSpan.FromMinutes(1), create);
            string secondValue = cache.GetOrCreate("key", TimeSpan.FromMinutes(1), create);

            Assert.Equal("created", secondValue);
            Assert.Equal(1, createCount);
        }

        [Fact]
        public void Remove_DeletesTheValue()
        {
            MemoryCacheService cache = new MemoryCacheService(this._clock);
            cache.Store("name", "Eric", TimeSpan.FromMinutes(5));

            Assert.True(cache.Remove("name"));
            Assert.False(cache.TryGet("name", out string? _));
        }

        [Fact]
        public void Store_RejectsLifetimeOfZero()
        {
            MemoryCacheService cache = new MemoryCacheService(this._clock);

            Assert.Throws<ArgumentOutOfRangeException>(() => cache.Store("name", "Eric", TimeSpan.Zero));
        }
    }
}
