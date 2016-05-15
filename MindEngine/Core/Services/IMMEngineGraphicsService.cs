namespace MindEngine.Core.Services
{
    using Graphics;
    using Microsoft.Xna.Framework.Graphics;

    public interface IMMEngineGraphicsService
    {
        #region Manager and Settings

        IMMGraphicsManager Manager { get; }

        IMMGraphicsSettings Settings { get; }

        #endregion

        #region Renderer

        IMMRenderer Renderer { get; }

        IMMGraphicsDeviceController DeviceController { get; }

        GraphicsDevice Device { get; }

        #endregion
    }
}