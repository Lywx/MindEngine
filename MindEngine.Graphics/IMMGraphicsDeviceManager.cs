namespace MindEngine.Graphics
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public interface IMMGraphicsDeviceManager : IGameComponent, IGraphicsDeviceService, IGraphicsDeviceManager, IDisposable
    {
        #region Configuration Operations

        void CenterWindow();

        #endregion
    }
}