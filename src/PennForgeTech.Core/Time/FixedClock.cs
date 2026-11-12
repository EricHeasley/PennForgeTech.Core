using System;

namespace PennForgeTech.Core.Time
{
    public sealed class FixedClock : IClock
    {
        public FixedClock(DateTimeOffset utcNow)
        {
            this.UtcNow = utcNow;
        }

        public DateTimeOffset UtcNow { get; private set; }

        public void Advance(TimeSpan amount)
        {
            this.UtcNow = this.UtcNow.Add(amount);
        }
    }
}
