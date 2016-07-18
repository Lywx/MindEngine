namespace MindEngine.Input.Mouse
{
    public class MMMouseButtonState
    {
        public MMMouseButtonState(MMMouseButton button)
        {
            this.Button = button;
        }

        /// <summary>
        ///     Unique button identification.
        /// </summary>
        public MMMouseButton Button { get; }

        /// <summary>
        ///     Whether the button has been identified as pressed.
        /// </summary>
        public bool Pressed { get; set; }

        public ulong PressedDuration { get; set; }

        /// <summary>
        /// Check if a button was just pressed in the most recent update.
        /// </summary>
        public bool Down { get; set; }

        /// <summary>
        /// Check if a button was just released in the most recent update.
        /// </summary>
        public bool Up { get; set; }
    }
}
