namespace MindEngine.Input.Mouse.EventArgs
{
    using System;
    using System.Windows.Forms;
    using Microsoft.Xna.Framework;

    public class MMMouseEventArgs : EventArgs
    {
        public MMMouseEventArgs(MMMouseButtons button, int clicks, int x, int y, int delta)
        {
            this.Button = button;
            this.Clicks = clicks;
            this.X = x;
            this.Y = y;
            this.Delta = delta;
        }

        public MMMouseButtons Button { get; }

        public int Clicks { get; }

        public int Delta { get; }

        public Vector2 Location => new Vector2(this.X, this.Y);

        public int X { get; }

        public int Y { get; }

        public static implicit operator MMMouseEventArgs(MouseEventArgs args)
        {
            return new MMMouseEventArgs(
                (MMMouseButtons)
                args.Button,
                args.Clicks,
                args.X,
                args.Y,
                args.Delta);
        }
    }
}
