using System;

namespace PennForgeTech.Core.Extensions
{
    public static class DateTimeOffsetExtensions
    {
        public static DateTimeOffset StartOfDay(this DateTimeOffset value)
        {
            return new DateTimeOffset(value.Year, value.Month, value.Day, 0, 0, 0, value.Offset);
        }

        public static DateTimeOffset EndOfDay(this DateTimeOffset value)
        {
            return value.StartOfDay().AddDays(1).AddTicks(-1);
        }

        public static DateTimeOffset StartOfMonth(this DateTimeOffset value)
        {
            return new DateTimeOffset(value.Year, value.Month, 1, 0, 0, 0, value.Offset);
        }

        public static bool IsWeekend(this DateTimeOffset value)
        {
            return value.DayOfWeek == DayOfWeek.Saturday || value.DayOfWeek == DayOfWeek.Sunday;
        }
    }
}
