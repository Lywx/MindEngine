namespace MindEngine.Core.Service
{
    using Graphics;
    using Microsoft.Xna.Framework.Graphics;

    public interface IMMEngineGraphicsService
    {
        #region Device

        IMMGraphicsDeviceManager DeviceManager { get; }

        MMGraphicsDeviceSetting DeviceSetting { get; }

        IMMGraphicsDeviceController DeviceController { get; }

        GraphicsDevice Device { get; }

        #endregion

        MMGraphicsRenderer Renderer { get; }
    }
}