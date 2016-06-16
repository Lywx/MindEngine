namespace MindEngine.Core.Scenes.Node.Action.Interval
{
    using System;
    using System.Linq;

    public class MMSequence : MMFiniteTimeAction
    {
        #region Constructors

        public MMSequence(
            MMFiniteTimeAction action1,
            MMFiniteTimeAction action2)
            : base(action1.Duration + action2.Duration)
        {
            this.InitializeActions(action1, action2);
        }

        /// <remarks>
        ///     Can't call base(duration) because we need to calculate duration here
        /// </remarks>
        public MMSequence(params MMFiniteTimeAction[] actions)
            : base(actions.Sum(action => action.Duration))
        {
            if (actions.Length == 1)
            {
                this.InitializeActions(actions[0], new MMExtraAction());
            }
            else
            {
                // Initial nested action is the first action
                var actionNested = actions[0];

                // Create a whole bunch of nested sequences from the actions.
                //
                // Start from 2nd action because the first action is nested
                // already. End with the backward counted 2nd actions, for the
                // last action should be used in InitializeActions(actionNested, actoinLast)
                for (var i = 1; i < actions.Length - 1; ++i)
                {
                    // Note that it is recursive construction of MMSequence.
                    actionNested = new MMSequence(actionNested, actions[i]);
                }

                this.InitializeActions(actionNested, actions[actions.Length - 1]);
            }
        }

        #endregion Constructors

        public MMFiniteTimeAction[] Actions { get; private set; }

        #region Initialization

        private void InitializeActions(
            MMFiniteTimeAction action1,
            MMFiniteTimeAction action2)
        {
            if (action1 == null)
            {
                throw new ArgumentNullException(nameof(action1));
            }

            if (action2 == null)
            {
                throw new ArgumentNullException(nameof(action2));
            }

            this.Actions = new MMFiniteTimeAction[2]
            {
                action1,
                action2
            };
        }

        #endregion Initialization

        #region Operations

        protected internal override MMActionState StartAction(IMMNode target)
        {
            return new MMSequenceState(this, target);
        }

        public override MMFiniteTimeAction Reverse()
        {
            // Note that order of actions is reversed
            return new MMSequence(
                this.Actions[1].Reverse(),
                this.Actions[0].Reverse());
        }

        #endregion Operations
    }
}