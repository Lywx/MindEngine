namespace MindEngine.Core.Service
{
    using System;
    using Component;
    using Graphics;
    using Microsoft.Xna.Framework.Graphics;

    public class MMEngineGraphics : MMCompositeComponent, IMMEngineGraphics
    {
        public MMEngineGraphics(MMEngine engine)
            : base(engine)
        {
            if (engine == null)
            {
                throw new ArgumentNullException(nameof(engine));
            }

            this.DeviceManager = new MMGraphicsDeviceManager(engine);
            this.DeviceController = new MMGraphicsDeviceController(engine);

            this.Renderer = new MMGraphicsRenderer(engine, this.DeviceController);
        }

        #region Initialization

        public override void Initialize()
        {
            this.DeviceManager.Initialize();
            this.DeviceController.Initialize();
            
            this.Renderer.Initialize();

            this.Cursor = new MMCursorDevice(this.EngineInterop.Asset.Cursors["Entis"]);

            base.Initialize();
        }

        #endregion

        #region Device

        public IMMGraphicsDeviceManager DeviceManager { get; private set; }

        public MMGraphicsDeviceSetting DeviceSetting { get; }

        public GraphicsDevice Device => this.DeviceManager.GraphicsDevice;

        public IMMGraphicsDeviceController DeviceController { get; private set; }

        #endregion

        public MMGraphicsRenderer Renderer { get; }

        /// <remarks>
        /// The default behavior is not initializing cursor. The user need to initialize their own cursor.
        /// </remarks>
        public MMCursorDevice Cursor { get; set; }

        #region IDisposable

        private bool IsDisposed { get; set; }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (!this.IsDisposed)
                    {
                        this.DeviceController?.Dispose();
                        this.DeviceController = null;
                    }

                    this.IsDisposed = true;
                }
            }
            catch
            {
                // Ignored
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        #endregion
    }
}
