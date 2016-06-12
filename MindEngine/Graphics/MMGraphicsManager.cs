namespace MindEngine.Graphics
{
    using System;
    using Core;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class MMGraphicsManager : GraphicsDeviceManager, IMMGraphicsManager
    {
        #region Dependency

        private MMEngine Engine { get; set; }

        private MMGraphicsSettings Settings { get; set; }

        #endregion

        public MMGraphicsManager(MMEngine engine, MMGraphicsSettings settings)
            : base(engine)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            this.Engine   = engine;
            this.Settings = settings;

            this.CreateDevice();

            // Set default resolution
            this.PreferredBackBufferWidth  = 800;
            this.PreferredBackBufferHeight = 600;

            this.ApplyChanges();
        }

        #region Initialization

        public void Initialize()
        {
            this.ApplySettings(this.Settings);
            this.CentralizeWindow(this.Settings);
        }

        #endregion

        #region Window Operations

        /// <param name="settings"></param>
        /// <remarks>
        /// Can be only called after MMEngine is constructed.
        /// </remarks>>
        private void CentralizeWindow(MMGraphicsSettings settings)
        {
            var window = this.Engine.Window;
            var screen = settings.Screen;
            var bounds = screen.Bounds;

            window.Position = new Point(
                bounds.X + (bounds.Width - settings.Width) / 2,
                bounds.Y + (bounds.Height - settings.Height) / 2);
        }

        #endregion

        #region Setting Operations

        public void ApplySettings(MMGraphicsSettings settings)
        {
            // Fixed drawing order in 3d graphics
            this.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            this.ApplyFrameSettings(settings);
            this.ApplyScreenSettings(settings);
            
            this.ApplyChanges();
        }

        private void ApplyFrameSettings(MMGraphicsSettings settings)
        {
            this.Engine.TargetElapsedTime = TimeSpan.FromMilliseconds(1000 / (double)settings.Fps);
            this.Engine.IsFixedTimeStep = true;
        }

        private void ApplyScreenSettings(MMGraphicsSettings settings)
        {
            // Resolution
            this.PreferredBackBufferWidth  = settings.Width;
            this.PreferredBackBufferHeight = settings.Height;

            // Border
            var window = this.Engine.Window;
            window.IsBorderless = settings.IsWindowMode;
        }

        #endregion Graphics Operations
    }
}