namespace MindEngine.Core.Service.Event
{
    using System;

    public static class MMEventHelper
    {
        public static long SecondsToTicks(int second)
        {
            return second * TimeSpan.TicksPerSecond;
        }
    }
}