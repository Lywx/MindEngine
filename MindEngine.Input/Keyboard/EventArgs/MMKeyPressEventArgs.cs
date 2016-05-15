namespace MindEngine.Input.Keyboard.EventArgs
{
    using System;
    using System.Windows.Forms;
    using Keys = Microsoft.Xna.Framework.Input.Keys;

    public class MMKeyPressEventArgs : EventArgs
    {
        public MMKeyPressEventArgs(Keys keyChar)
        {
            this.KeyChar = keyChar;
        }

        public Keys KeyChar { get; }

        public static implicit operator MMKeyPressEventArgs(KeyPressEventArgs args)
        {
            return new MMKeyPressEventArgs((Keys)args.KeyChar);
        }
    }
}