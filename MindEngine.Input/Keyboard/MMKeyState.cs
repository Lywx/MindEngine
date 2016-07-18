namespace MindEngine.Input.Keyboard
{
    using Microsoft.Xna.Framework.Input;

    public class MMKeyState
    {
        public MMKeyState(Keys key)
        {
            this.Key = key;
        }

        public Keys Key { get; }

        public ulong PressedDuration { get; set; }

        public bool Pressed { get; set; }

        /// <summary>
        /// Check if a key was just pressed in the most recent update.
        /// </summary   
        public bool Down { get; set; }

        /// <summary>
        /// Check if a key was just released in the most recent update.
        /// </summary
        public bool Up { get; set; }
    }
}
