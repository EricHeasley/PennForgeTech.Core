using System;

namespace PennForgeTech.Core.Abstractions
{
    public sealed class SystemClock : IClock
    {
        public DateTimeOffset UtcNow
        {
            get { return DateTimeOffset.UtcNow; }
        }
    }
}
