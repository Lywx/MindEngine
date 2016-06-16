namespace MindEngine.Math.Geometry
{
    using System;
    using Microsoft.Xna.Framework;

    public interface IMMShape : IDisposable
    {
        event EventHandler<MMShapeBoundChangedEventArgs> Move;

        event EventHandler<MMShapeBoundChangedEventArgs> Resize;

        Rectangle Bound { get; }
    }
}