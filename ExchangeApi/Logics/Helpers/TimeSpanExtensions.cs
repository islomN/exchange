using System;

namespace ExchangeApi.Logics.Helpers
{
    public static class TimeSpanHelper
    {
        public static TimeSpan GetTimeUntilEndOfHour()
        {
            var now = DateTime.Now;
            var endHour = new DateTime(now.Year, now.Month, now.Day, now.Hour + 1, 0, 0);
            return endHour - now;
        }
    }
}