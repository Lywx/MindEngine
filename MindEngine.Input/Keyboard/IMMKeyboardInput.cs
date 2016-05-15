namespace MindEngine.Input.Keyboard
{
    using Core;
    using Microsoft.Xna.Framework.Input;

    public interface IMMKeyboardInput : IMMInputtableOperations
    {
        #region Action States

        /// <summary>
        /// Check if an action has been pressed.
        /// </summary>
        bool ActionPressed(MMInputAction action);

        /// <summary>
        /// Check if an action was just performed in the most recent update.
        /// </summary>
        bool ActionTriggered(MMInputAction action);

        #endregion

        #region Key States

        /// <summary>
        /// Check if a key is pressed.
        /// </summary>
        bool KeyPressed(Keys key);

        /// <summary>
        /// Check if a key was just pressed in the most recent update.
        /// </summary>
        bool KeyTriggered(Keys key);

        bool KeyAltDown { get; }

        bool KeyCtrlDown { get; }

        bool KeyShiftDown { get; }

        #endregion
    }
}