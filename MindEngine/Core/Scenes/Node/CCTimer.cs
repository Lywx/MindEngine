namespace MindEngine.Core.Scenes.Node
{
    using System;

    internal class CCTimer : IMMNodeUpdatable
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionScheduler"></param>
        /// <param name="target"></param>
        /// <param name="selector"></param>
        /// <param name="interval">Timer interval in seconds.</param>
        /// <param name="repeat">Timer extra repetition number.</param>
        /// <param name="delay">Timer delay for timing.</param>
        public CCTimer(
            CCActionScheduler actionScheduler,
            IMMNodeUpdatable target,
            Action<float> selector,
            float interval = 0f,
            uint repeat = 0,
            float delay = 0f)
        {
            this.ActionScheduler = actionScheduler;
            this.Target    = target;
            this.Selector  = selector;

            this.Interval         = interval;
            this.IntervalOriginal = interval;

            this.Delay = delay;
            this.DelayEnabled = this.Delay > 0f;

            // Default value for not updated  
            this.Elapsed = -1;

            this.Repeat     = repeat;
            this.RunForever = repeat == uint.MaxValue;
        }

        #endregion Constructors

        #region Interval Data

        /// <summary>
        /// Timer interval in seconds when it is constructed.
        /// </summary>
        public float IntervalOriginal { get; internal set; }

        /// <summary>
        /// Timer interval in seconds.
        /// </summary>
        public float Interval { get; set; }

        #endregion

        #region Repetition Data

        private uint invocationCount;

        /// <remarks>
        /// 0 = once, 1 is twice time executed, 2 is third time, etc.
        /// </remarks>
        public uint Repeat { get; }

        public bool RunForever { get; }

        #endregion

        #region Timer Data

        private bool elapsedEverUpdated;

        public float Elapsed { get; private set; }

        public float Delay { get; }

        public bool DelayEnabled { get; private set; }

        #endregion

        #region

        public Action<float> Selector { get; set; }

        private CCActionScheduler ActionScheduler { get; }

        public IMMNodeUpdatable Target { get; }

        #endregion

        #region Update

        public void Update(float dt)
        {
            // When update for the first time
            if (this.elapsedEverUpdated)
            {
                this.elapsedEverUpdated = true;

                this.Reset();

                return;
            }

            this.UpdateElapsed(dt);

            if (this.DelayEnabled)
            {
                this.TryFinishDelay();
            }
            else
            {
                this.TryFinish();
            }

            if (!this.RunForever)
            {
                this.TryUnschedule();
            }
        }

        private void UpdateElapsed(float dt)
        {
            this.Elapsed += dt;
        }

        #endregion

        #region 

        private void TryFinishDelay()
        {
            if (this.Elapsed >= this.Delay)
            {
                this.InvokeDelay();

                this.DelayEnabled = false;
            }
        }

        private void TryFinish()
        {
            if (this.Elapsed >= this.Interval)
            {
                this.InvokeHit();
            }
        }

        private void TryUnschedule()
        {
            if (this.invocationCount > this.Repeat)
            {
                this.ActionScheduler.Unschedule(this.Selector, this.Target);
            }
        }

        #endregion

        #region Invoke Operations

        private void InvokeSelector()
        {
            this.Selector?.Invoke(this.Elapsed);

            this.invocationCount += 1;
        }

        private void InvokeHit()
        {
            this.InvokeSelector();

            this.ResetElapsed();
        }

        private void InvokeDelay()
        {
            this.InvokeSelector();

            this.Elapsed = this.Elapsed - this.Delay;
        }

        #endregion

        #region Reset Operations

        private void Reset()
        {
            this.ResetElapsed();
            this.ResetCount();
        }

        private void ResetCount()
        {
            this.invocationCount = 0;
        }

        private void ResetElapsed()
        {
            this.Elapsed = 0;
        }  

        #endregion
    }
}
