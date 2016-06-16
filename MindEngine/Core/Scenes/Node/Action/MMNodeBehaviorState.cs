namespace MindEngine.Core.Scenes.Node.Action
{
    using System;
    using Behavior;
    using Services.Processes;

    public interface IMMNodeBehaviorState
    {
        
    }

    /// <summary>
    /// ActionState is a base class for various ActionState pairing Action. It 
    /// contains the necessary information for running of the pairing Action.
    /// </summary>
    public abstract class MMNodeBehaviorState : MMProcess
    {
        #region Constructors

        /// <param name="action"></param>
        /// <param name="target">When target is null, the actions is stopped.</param>
        public MMNodeBehaviorState(MMNodeBehavior action, MMNode target)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            this.Action = action;

            this.Target         = target;
            this.TargetOriginal = target;
        }

        #endregion

        #region Action Data

        public MMNodeBehavior Action { get; protected set; }

        #endregion

        #region Target Data

        /// <summary>
        ///     Gets or sets the target.
        ///     Will be set with the 'StartAction' method of the corresponding Action.
        ///     When the 'Stop' method is called, Target will be set to null.
        /// </summary>
        /// <value>The target.</value>
        /// <remarks>
        ///     When target is null, the action is stopped.
        /// </remarks>
        public MMNode Target { get; protected set; }

        public MMNode TargetOriginal { get; protected set; }

        #endregion 

        #region State Data

        /// <summary>
        ///     Gets a value indicating whether this instance is done.
        /// </summary>
        /// <value><c>true</c> if this instance is done; otherwise, <c>false</c>.</value>
        public virtual bool IsDone => true;

        #endregion

        public override void OnExit()
        {
            this.Target = null;
        }

        /// <summary>
        ///     Called every frame with it's delta time.
        ///     DON'T override unless you know what you are doing.
        /// </summary>
        /// <param name="dt">Delta Time</param>
        protected internal virtual void Step(float dt)
        {
        }

        /// <summary>
        ///     Called once per frame by this.Step()
        /// </summary>
        /// <param name="percent">
        ///     A value between 0 and 1
        ///     For example:
        ///     0 means that the action just started
        ///     0.5 means that the action is in the middle
        ///     1 means that the action is over
        /// </param>
        public virtual void Progress(float percent)
        {
        }
    }
}