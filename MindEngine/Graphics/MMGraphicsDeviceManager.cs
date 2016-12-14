namespace MindEngine.Graphics
{
    using System;
    using Core;
    using Microsoft.Xna.Framework;

    public class MMGraphicsDeviceManager : GraphicsDeviceManager, IMMGraphicsDeviceManager 
    {
        #region Constructors and Finalizer

        public MMGraphicsDeviceManager(MMEngine engine)
            : base(engine)
        {
            this.Engine = engine;

            // Create graphics device according to the standard
            this.CreateDevice();

            // Set default resolution
            this.PreferredBackBufferWidth = 800;
            this.PreferredBackBufferHeight = 600;

            this.ApplyChanges();
        }

        #endregion

        MMEngine Engine { get; set; }

        MMGraphicsDeviceSetting setting;

        public MMGraphicsDeviceSetting Setting
        {
            get { return this.setting; }
            set { this.ApplySetting(value); }
        }

        public MMGraphicsDeviceContext Context { get; private set; }

        #region Initialization

        public void Initialize()
        {
            var availableResolution = this.Engine.Interop..Configuration.Get<>
            this.Context = new MMGraphicsDeviceContext(this.);
            this.Setting = 

            this.ApplySetting(this.Setting);
            this.CenterWindow();
        }

        #endregion

        #region Operations

        public void CenterWindow()
        {
            var window = this.Engine.Window;
            var screen = this.Setting.Screen;
            var bounds = screen.Bounds;

            window.Position = new Point(
                bounds.X + (bounds.Width - this.Setting.Width) / 2,
                bounds.Y + (bounds.Height - this.Setting.Height) / 2);
        }

        #endregion

        #region Setting Operations

        private void ApplySetting(MMGraphicsDeviceSetting settingSubmitted)
        {
            this.ApplyFrameSettings(settingSubmitted);
            this.ApplyScreenSettings(settingSubmitted);
            this.ApplyChanges();

            this.setting = settingSubmitted;
        }

        private void ApplyFrameSettings(MMGraphicsDeviceSetting settingSubmitted)
        {
            this.Engine.TargetElapsedTime = TimeSpan.FromMilliseconds(1000 / (double)settingSubmitted.Fps);
            this.Engine.IsFixedTimeStep = true;
        }

        private void ApplyScreenSettings(MMGraphicsDeviceSetting settingSubmitted)
        {
            // Resolution
            this.PreferredBackBufferWidth  = settingSubmitted.Width;
            this.PreferredBackBufferHeight = settingSubmitted.Height;

            // Border
            var window = this.Engine.Window;
            window.IsBorderless = settingSubmitted.IsBorderless;
        }

        #endregion
    }
}