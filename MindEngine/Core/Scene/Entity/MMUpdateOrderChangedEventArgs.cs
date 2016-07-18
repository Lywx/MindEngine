namespace MindEngine.Core.Scene.Entity
{
    using System;

    public class MMUpdateOrderChangedEventArgs : EventArgs
    {
        public MMUpdateOrderChangedEventArgs(int updapteOrder)
        {
            this.UpdapteOrder = updapteOrder;
        }

        public int UpdapteOrder { get; }
    }
}
