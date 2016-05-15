namespace MindEngine.Core.Components
{
    using System;
    using Microsoft.Xna.Framework.Graphics;

    public interface IMMDrawableComponent : IMMGameComponent, IMMDrawableComponentOperations
    {
        GraphicsDevice GraphicsDevice { get; }

        event EventHandler<EventArgs> DrawOrderChanged;

        event EventHandler<EventArgs> VisibleChanged;

        int DrawOrder { get; }

        bool Visible { get; }
    }
}