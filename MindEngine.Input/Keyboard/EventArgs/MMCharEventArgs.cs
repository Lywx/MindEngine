namespace MindEngine.Input.Keyboard.EventArgs
{
    using System;
    using Microsoft.Xna.Framework;

    public class MMCharEventArgs : EventArgs
    {
        public MMCharEventArgs(char @char)
        {
            this.Char = @char;
        }

        public char Char { get; }

        public static implicit operator MMCharEventArgs(TextInputEventArgs args)
        {
            return new MMCharEventArgs(args.Character);
        }
    }
}
