namespace MindEngine.Core.Service
{
    using System;
    using Graphics;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Provide a wrapper around the service in order to hot swap the core 
    /// module in engine.
    /// </summary>
    public sealed class MMEngineGraphicsService : IMMEngineGraphicsService
    {
        public MMEngineGraphicsService(IMMEngineGraphics graphics)
        {
            if (graphics == null)
            {
                throw new ArgumentNullException(nameof(graphics));
            }

            this.Graphics = graphics;
        }

        IMMEngineGraphics Graphics { get; }

        #region Device

        public IMMGraphicsDeviceManager DeviceManager => this.Graphics.DeviceManager;

        public IMMGraphicsDeviceController DeviceController => this.Graphics.DeviceController;

        public GraphicsDevice Device => this.DeviceManager.GraphicsDevice;

        public MMGraphicsDeviceSetting DeviceSetting => this.Graphics.DeviceSetting;

        #endregion

        #region 

        public MMGraphicsRenderer Renderer => this.Graphics.Renderer;

        #endregion
    }
}
