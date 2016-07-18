namespace MindEngine.Input.Gamepad
{
    public class MMGamePadButtonState
    {
        public MMGamePadButtonState(MMGamePadButton button)
        {
            this.Button = button;
        }

        public MMGamePadButton Button { get; set; }

        public ulong PressedDuration { get; set; }

        public bool Pressed { get; set; }

        /// <summary>
        ///     Check if a key was just pressed in the most recent update.
        /// </summary
        public bool Down { get; set; }

        /// <summary>
        ///     Check if a key was just released in the most recent update.
        /// </summary
        public bool Up { get; set; }
    }
}
