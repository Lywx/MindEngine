namespace MindEngine.Core.Service
{
    using Input;
    using Input.Keyboard;
    using Input.Mouse;

    public interface IMMEngineInputService : IMMInputtableOperations
    {
        IMMKeyboardInput Keyboard { get; }

        IMMMouseInput Mouse { get; }

        // IMMGamepadInput Gamepad { get; }

        #region Actions

        /// <summary>
        /// Check if an action has been pressed.
        /// </summary>
        bool ActionPressed(MMInputAction action);

        /// <summary>
        /// Return the duration in ticks of an action has been pressed.
        /// </summary>
        ulong ActionPressedDuration(MMInputAction action);

        /// <summary>
        /// Check if an action was just pressed in the most recent update.
        /// </summary>
        bool ActionDown(MMInputAction action);

        /// <summary>
        /// Check if an action was just released in the most recent update.
        /// </summary>
        bool ActionUp(MMInputAction action);

        #endregion

        #region Action Bindings

        void LoadKeyboardBinding<TActions>() where TActions : MMInputActions;

        // void LoadMouseBinding<TActions>() where TActions : MMInputActions;

        // void LoadGamepadBinding<TActions>() where TActions : MMInputActions;

        #endregion
    }
}
