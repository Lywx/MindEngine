namespace MindEngine.Core.Scene.Entity
{
    using System;

    public class MMDrawEnabledChangedEventArgs : EventArgs
    {
        public MMDrawEnabledChangedEventArgs(bool drawEnabled)
        {
            this.DrawEnabled = drawEnabled;
        }

        public bool DrawEnabled { get; }
    }
}