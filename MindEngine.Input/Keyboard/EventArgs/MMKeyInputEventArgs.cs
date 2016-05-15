namespace MindEngine.Input.Keyboard.EventArgs
{
    using System;
    using Microsoft.Xna.Framework;

    public class MMKeyInputEventArgs : EventArgs
    {
        public MMKeyInputEventArgs(char key)
        {
            this.Key = key;
        }

        public char Key { get; }

        public static implicit operator MMKeyInputEventArgs(TextInputEventArgs args)
        {
            return new MMKeyInputEventArgs(args.Character);
        }
    }
}
