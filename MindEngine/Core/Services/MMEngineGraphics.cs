namespace MindEngine.Core.Services
{
    using System;
    using Graphics;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class MMEngineGraphics : DrawableGameComponent, IMMEngineGraphics
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
            this.Manager  = new MMGraphicsManager(engine, settings);

            //TODO(Wuxiang)
            // No dependency injection here, because sprite batch is never replaced as long 
            // as this is a MonoGame application.
            this.DeviceController = new MMGraphicsDeviceController(engine);
            //this.Game.Components.Add(this.DeviceController);

            //TODO(Wuxiang)
            // No dependency injection here, because string drawer is a class focus on string 
            // drawing. The functionality is never extended in the form of inheritance.
            //this.Renderer = new MMRenderer(this.DeviceController);
        }

        public GraphicsDevice Device => this.Manager.GraphicsDevice;

        #region Setting Data

        public IMMGraphicsManager Manager { get; private set; }
        
        public IMMGraphicsSettings Settings { get; private set; }

        #endregion

        #region Render Data

        public IMMGraphicsDeviceController DeviceController { get; private set; }

        public IMMRenderer Renderer { get; private set; }

        #endregion

        #region Initialization

        public override void Initialize()
        {
            //TODO(Wuxiang)
            //this.Manager.Initialize();
        }

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
                        //TODO(Wuxiang)
                        //this.Manager?.Dispose();
                        this.Manager = null;

                        //TODO(Wuxiang)
                        //this.DeviceController?.Dispose();
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