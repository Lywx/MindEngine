namespace MindEngine.Core.Scenes.Node.Behavior
{
    using Action;

    /// <summary>
    /// Actions are a set of transform you could apply to Node object. Action 
    /// class contains only data that characterizes the specific Action.
    /// </summary>
    public abstract class MMNodeBehavior : MMObject
    {
        #region Constructors and Finalizer

        protected MMNodeBehavior()
        {
        }

        #endregion Constructor

        #region Operations

        protected internal virtual MMNodeBehaviorState StartAction(IMMNode target)
        {
            return null;
        }

        #endregion
    }
}
