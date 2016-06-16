namespace MindEngine.Core.Scenes.Node.Action.Interval
{
    using System;

    public interface IMMActionTweenDelegate
    {
        void UpdateTweenAction(float value, string key);
    }

    public class MMActionTween : MMFiniteTimeAction
    {
        #region Constructors

        /// <summary>
        /// It is an action that lets you update any property of an object.
        /// </summary>
        public MMActionTween(float duration, string key, float keyFrom, float keyTo)
            : base(duration)
        {
            this.Key     = key;
            this.KeyFrom = keyFrom;
            this.KeyTo   = keyTo;
        }

        /// <summary>
        /// Provides another way to implement the tween action using Action<float, float> instead of IMMActionnTweenDelegate.
        /// </summary>
        public MMActionTween(
            float duration,
            string key,
            float keyFrom,
            float keyTo,
            Action<float, string> tweenAction) : this(duration, key, keyFrom, keyTo)
        {
            this.TweenAction = tweenAction;
        }

        #endregion Constructors

        #region Properties

        public string Key { get; private set; }

        public float KeyFrom { get; private set; }

        public float KeyTo { get; private set; }

        public Action<float, string> TweenAction { get; private set; }

        #endregion Properties

        protected internal override MMActionState StartAction(IMMNode target)
        {
            return new MMActionTweenState(this, target);
        }

        public override MMFiniteTimeAction Reverse()
        {
            return new MMActionTween(
                this.Duration,
                this.Key,
                this.KeyTo,
                this.KeyFrom,
                this.TweenAction);
        }
    }

    public class MMActionTweenState : MMFiniteTimeActionState
    {
        protected float Delta;

        protected float From { get; private set; }

        protected float To { get; private set; }

        protected string Key { get; private set; }

        protected Action<float, string> TweenAction { get; private set; }

        public MMActionTweenState(MMActionTween action, IMMNode target)
            : base(action, target)
        {
            // TODO(Critical): Maybe I should use dynamic binding here
            if (!(target is IMMActionTweenDelegate))
            {
                throw new ArgumentException("target must implement MMActionTweenDelegate");
            }

            this.TweenAction = action.TweenAction;
            this.From        = action.KeyFrom;
            this.To          = action.KeyTo;
            this.Key         = action.Key;
            this.Delta       = this.To - this.From;
        }

        public override void Progress(float percent)
        {
            var value = this.To - this.Delta * (1 - percent);

            if (this.TweenAction != null)
            {
                this.TweenAction(value, this.Key);
            }
            else if (this.Target is IMMActionTweenDelegate)
            {
                ((IMMActionTweenDelegate)this.Target).UpdateTweenAction(value, this.Key);
            }
        }
    }
}