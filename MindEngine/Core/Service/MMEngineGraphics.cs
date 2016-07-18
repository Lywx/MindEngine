namespace MindEngine.Core.Service
{
    using System;
    using Component;
    using Graphics;
    using Microsoft.Xna.Framework.Graphics;

    public class MMEngineGraphics : MMCompositeComponent, IMMEngineGraphics
    {
        public MMEngineGraphics(MMEngine engine, MMGraphicsSettings settings)
            : base(engine)
        {
            if (engine == null)
            {
                throw new ArgumentNullException(nameof(engine));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            this.Settings = settings;
            this.Manager = new MMGraphicsManager(engine, settings);

            // No dependency injection here, because sprite batch is never replaced as long 
            // as this is a MonoGame application.
            this.DeviceController = new MMGraphicsDeviceController(engine);

            // No dependency injection here, because string drawer is a class focus on string 
            // drawing. The functionality is never extended in the form of inheritance.
            this.Renderer = new MMRenderer(engine, this.DeviceController);
        }

        public GraphicsDevice Device => this.Manager.GraphicsDevice;

        /// <remarks>
        /// The default behavior is not initializing cursor. The user need to initialize their own cursor.
        /// </remarks>
        public MMCursorDevice Cursor { get; set; }

        #region Initialization

        public override void Initialize()
        {
            this.Manager.Initialize();
            this.DeviceController.Initialize();
            this.Renderer.Initialize();

            this.Cursor = new MMCursorDevice(this.EngineInterop.Asset.Cursors["Entis"]);

            base.Initialize();
        }

        #endregion

        #region Setting Data

        public IMMGraphicsManager Manager { get; private set; }

        public IMMGraphicsSettings Settings { get; }

        #endregion

        #region Render Data

        public IMMGraphicsDeviceController DeviceController { get; private set; }

        public MMRenderer Renderer { get; }

        #endregion

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
