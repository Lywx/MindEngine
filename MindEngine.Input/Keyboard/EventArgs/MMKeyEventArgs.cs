namespace MindEngine.Input.Keyboard.EventArgs
{
    using System;
    using System.Windows.Forms;
    using Keys = Microsoft.Xna.Framework.Input.Keys;

    public class MMKeyEventArgs : EventArgs
    {
        public MMKeyEventArgs(Keys key)
        {
            this.Key = key;
        }

        private MMKeyEventArgs(System.Windows.Forms.Keys key)
        {
            this.Key = (Keys)key;
        }

        public Keys Key { get; }

        public static implicit operator MMKeyEventArgs(KeyEventArgs args)
        {
            return new MMKeyEventArgs(args.KeyCode);
        }

        public static implicit operator MMKeyEventArgs(KeyPressEventArgs args)
        {
            return new MMKeyEventArgs((Keys)args.KeyChar);
        }
    }
}