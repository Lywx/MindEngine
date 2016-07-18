namespace MindEngine.Core.Service
{
    using System;
    using Graphics;
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

        public MMCursorDevice Cursor => this.Graphics.Cursor;

        public MMRenderer Renderer => this.Graphics.Renderer;

        public IMMGraphicsDeviceController DeviceController => this.Graphics.DeviceController;

        #endregion
    }
}
