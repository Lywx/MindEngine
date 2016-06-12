namespace MindEngine.Core.Services.Debug
{
    using System;
    using System.Diagnostics;

    public class MMDebugBlockTimer : IDisposable
    {
        // TODO(Wuxiang)
        // private int timerId;

        private Stopwatch timer;

        private MMDebugBlockTimerRecord timerRecord;

        public MMDebugBlockTimer(
            [System.Runtime.CompilerServices.CallerFilePath]   string callerFilePath = "",
            [System.Runtime.CompilerServices.CallerMemberName] string callerMemberName = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int    callerLineNumber = 0)
        {
#if DEBUG
            this.timerRecord = new MMDebugBlockTimerRecord(callerFilePath, callerMemberName, callerLineNumber);

            this.BeginTiming();
#endif
        }

        private void BeginTiming()
        {
            this.timer = new Stopwatch();
            this.timer.Start();
        }

        private void EndTiming()
        {
            this.timer.Stop();
            this.timerRecord.CallerTimestamp = this.timer.ElapsedMilliseconds;
        }

        public void Dispose()
        {
#if DEBUG
            this.EndTiming();

            this.timer = null;
#endif
        }
    }
}