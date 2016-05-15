namespace MindEngine.Core.Services
{
    using System;
    using Microsoft.Xna.Framework.Graphics;

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

        private IMMEngineGraphics Graphics { get; }

        #region Manager and Settings

        public IMMGraphicsManager Manager => this.Graphics.Manager;

        public IMMGraphicsSettings Settings => this.Graphics.Settings;

        #endregion

        #region Renderer

        public GraphicsDevice Device => this.Manager.GraphicsDevice;

        public IMMRenderer Renderer => this.Graphics.Renderer;

        public IMMGraphicsDeviceController DeviceController => this.Graphics.DeviceController;

        #endregion
    }
}