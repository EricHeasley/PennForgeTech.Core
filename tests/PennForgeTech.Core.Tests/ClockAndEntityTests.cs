using System;
using PennForgeTech.Core.Domain;
using PennForgeTech.Core.Time;
using Xunit;

namespace PennForgeTech.Core.Tests
{
    public sealed class ClockAndEntityTests
    {
        private sealed class Customer : Entity<Guid>
        {
            public Customer(Guid id)
                : base(id)
            {
            }
        }

        private sealed class Supplier : Entity<Guid>
        {
            public Supplier(Guid id)
                : base(id)
            {
            }
        }

        [Fact]
        public void FixedClock_AdvancesByTheGivenAmount()
        {
            DateTimeOffset start = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
            FixedClock clock = new FixedClock(start);

            clock.Advance(TimeSpan.FromHours(2));

            Assert.Equal(start.AddHours(2), clock.UtcNow);
        }

        [Fact]
        public void Entities_WithSameTypeAndId_AreEqual()
        {
            Guid id = Guid.NewGuid();

            Assert.Equal(new Customer(id), new Customer(id));
        }

        [Fact]
        public void Entities_WithSameIdButDifferentType_AreNotEqual()
        {
            Guid id = Guid.NewGuid();

            Assert.NotEqual<object>(new Customer(id), new Supplier(id));
        }
    }
}
