namespace MindEngine.Input.Gamepad
{
    using Core;

    public interface IMMGamepadInput : IMMInputtableOperations
    {
        #region Key States

        /// <summary>
        /// Check if a key is pressed.
        /// </summary>
        bool ButtonPressed(MMGamePadButton button);

        /// <summary>
        /// Return the duration in ticks of a key has been pressed.
        /// </summary>
        ulong ButtonPressedDuration(MMGamePadButton key);

        /// <summary>
        /// Check if a key was just pressed in the most recent update.
        /// </summary>
        bool ButtonDown(MMGamePadButton key);

        /// <summary>
        /// Check if a key was just released in the most recent update.
        /// </summary>
        bool ButtonUp(MMGamePadButton key);

        #endregion
    }
}