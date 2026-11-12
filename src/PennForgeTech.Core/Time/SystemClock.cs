using System;

namespace PennForgeTech.Core.Time
{
    public sealed class SystemClock : IClock
    {
        public DateTimeOffset UtcNow
        {
            get { return DateTimeOffset.UtcNow; }
        }
    }
}
