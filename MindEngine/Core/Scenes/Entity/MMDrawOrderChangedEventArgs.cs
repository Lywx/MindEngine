namespace MindEngine.Core.Scenes.Entity
{
    using System;

    public class MMDrawOrderChangedEventArgs : EventArgs
    {
        public MMDrawOrderChangedEventArgs(int drawOrder)
        {
            this.DrawOrder = drawOrder;
        }

        public int DrawOrder { get; }
    }
}