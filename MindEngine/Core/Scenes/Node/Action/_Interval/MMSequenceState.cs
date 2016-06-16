namespace MindEngine.Core.Scenes.Node.Action.Interval
{
    using System;

    public class MMSequenceState : MMFiniteTimeActionState
    {
        private readonly MMFiniteTimeAction[] actions = new MMFiniteTimeAction[2];

        private readonly MMFiniteTimeActionState[] actionStates = new MMFiniteTimeActionState[2];

        private int actionIndexPrevious = -1;

        /// <summary>
        /// Update parameter split for the first action and second action.
        /// Because MMSequence is a combination of two actions.
        /// </summary>
        private float actionTimeSplit;

        #region Constructors

        public MMSequenceState(MMSequence action, IMMNode target)
            : base(action, target)
        {
            this.actions = action.Actions;

            this.actionTimeSplit = this.actions[0].Duration / this.Duration;
        }

        #endregion Constructors

        public override bool IsDone
        {
            get
            {
                if (this.HasActionRepeatForever
                    && this.PreviousActionRepeatForever)
                {
                    return false;
                }

                return base.IsDone;
            }
        }

        private bool HasActionEverRan => this.actionIndexPrevious != -1;

        protected bool HasActionRepeatForever
            => (this.actions[0] is MMRepeatForever)
               || (this.actions[1] is MMRepeatForever);

        protected bool PreviousActionRepeatForever
            => this.PreviousAction is MMRepeatForever;

        private MMFiniteTimeActionState PreviousActionState => this.actionStates[this.actionIndexPrevious];

        private MMFiniteTimeAction PreviousAction => this.actions[this.actionIndexPrevious];

        #region Operations

        protected internal override void Stop()
        {
            if (this.HasActionEverRan)
            {
                this.PreviousActionState.Stop();
            }

            base.Stop();
        }

        protected internal override void Step(float dt)
        {
            if (this.HasActionEverRan

                // When action in last frame will persist forever
                && this.PreviousActionRepeatForever)
            {
                this.PreviousActionState.Step(dt);
            }
            else
            {
                base.Step(dt);
            }
        }

        #endregion

        #region Update

        public override void Progress(float percent)
        {
            int   actionIndex;
            float actionTime;

            this.UpdateActionIndex(percent, out actionIndex);
            this.UpdateActionTime(percent, actionIndex, out actionTime);
            this.UpdateAction(actionIndex);
            this.UpdateActionChange(actionIndex, actionTime);
        }

        private void UpdateActionIndex(float time, out int actionIndex)
        {
            actionIndex = time < this.actionTimeSplit ? 0 : 1;
        }

        /// <summary>
        /// Update time parameter for updated action in this frame.
        /// </summary>
        /// <param name="time">
        /// </param>
        /// <param name="actionIndex">
        /// Updated action in this frame 
        /// </param>
        /// <param name="actionTime">
        /// Update parameter for updated action in this frame
        /// </param>
        private void UpdateActionTime(float time, int actionIndex, out float actionTime)
        {
            if (actionIndex == 0)
            {
                // Update progress for action[0], time = progress / completion 
                var action1TimeProcess = time;

                if (Math.Abs(this.actionTimeSplit) > float.Epsilon)
                {
                    actionTime = action1TimeProcess / this.actionTimeSplit;
                }
                else
                {
                    actionTime = 1;
                }
            }
            else
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator

                // When action[0]'s duration cover entire sequence
                if (this.actionTimeSplit == 1)
                {
                    // At a result, action[1] should be finished too
                    actionTime = 1;
                }
                else
                {
                    var action2TimeProgress = time - this.actionTimeSplit;
                    var action2TimeCompletion = 1 - this.actionTimeSplit;
                    actionTime = action2TimeProgress / action2TimeCompletion;
                }
            }
        }

        private void UpdateAction(int actionIndex)
        {
            if (actionIndex == 0)
            {
                // First time update
                if (this.actionIndexPrevious == -1)
                {
                    // Ignore
                }

                // When action[0] hasn't reached completion time and it is found
                // that time is reduced rather then increasing (reverse stepping)
                else if (this.actionIndexPrevious == 1)
                {
                    // "Reverse update" (reverse to 0f and stop)
                    this.actionStates[1].Progress(0);
                    this.actionStates[1].Stop();
                }
                else
                {
#if DEBUG
                    throw new InvalidOperationException();
#endif
                }
            }

            // When according to the time, action[0] should be finished
            else if (actionIndex == 1)
            {
                // However action[0] was skipped because the Update is not
                // called for duration being too short, do extra work to execute
                // skipped action[0]
                if (this.actionIndexPrevious == -1)
                {
                    this.actionStates[0] =
                        (MMFiniteTimeActionState)
                        this.actions[0].StartAction(this.Target);
                    this.actionStates[0].Progress(1.0f);
                    this.actionStates[0].Stop();
                }

                // When action[0] ran but fully update (1f) yet, do extra work
                // to fully update action[0]
                else if (this.actionIndexPrevious == 0)
                {
                    this.actionStates[0].Progress(1.0f);
                    this.actionStates[0].Stop();
                }
                else
                {
#if DEBUG
                    throw new InvalidOperationException();
#endif
                }
            }
        }

        private void UpdateActionChange(int actionIndex, float actionTime)
        {
            // When action index not changed and it is done, do nothing
            if (this.actionIndexPrevious == actionIndex 
                && this.actionStates[actionIndex].IsDone)
            {
                return;
            }

            // When action index changed, start next action.
            if (this.actionIndexPrevious != actionIndex)
            {
                this.actionStates[actionIndex] = (MMFiniteTimeActionState)this.actions[actionIndex].StartAction(this.Target);
            }

            this.actionStates[actionIndex].Progress(actionTime);

            this.actionIndexPrevious = actionIndex;
        }

        #endregion
    }
}