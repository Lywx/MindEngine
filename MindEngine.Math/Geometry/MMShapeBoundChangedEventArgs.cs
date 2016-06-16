namespace MindEngine.Math.Geometry
{
    using System;
    using Microsoft.Xna.Framework;

    public class MMShapeBoundChangedEventArgs : EventArgs
    {
        public MMShapeBoundChangedEventArgs(MMShapeEvent boundEvent, Rectangle boundPrevious, Rectangle boundCurrent)
        {
            this.BoundEvent    = boundEvent;
            this.BoundPrevious = boundPrevious;
            this.BoundCurrent  = boundCurrent;
        }

        public MMShapeEvent BoundEvent { get; }

        public Rectangle BoundCurrent { get; }

        public Rectangle BoundPrevious { get; }
    }
}
