namespace MindEngine.Core.Scene.Entity
{
    using System;

    public class MMUpdateEnabledChangedEventArgs : EventArgs
    {
        public MMUpdateEnabledChangedEventArgs(bool UpdateEnabled)
        {
            this.UpdateEnabled = UpdateEnabled;
        }

        public bool UpdateEnabled { get; }
    }
}
