namespace MindEngine.Core
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Service;

    public interface IMMEngineOperations
    {
        void Run();

        void Restart();
    }

    public interface IMMEngine : IDisposable, IMMEngineOperations
    {
        IMMEngineAudio Audio { get; } 

        IMMEngineGraphics Graphics { get; }

        IMMEngineInput Input { get; }

        IMMEngineInterop Interop { get; }

        IMMEngineNumerical Numerical { get; }

        IMMEngineDebug Debug { get; set; }
    }

    public class MMEngine : Game, IMMEngine
    {
        private IMMEngineAudio audio;

        private IMMEngineDebug debug;

        private IMMEngineGraphics graphics;

        private IMMEngineInput input;

        private IMMEngineInterop interop;

        private IMMEngineNumerical numerical;

        #region Constructors

        public MMEngine()
        {
            this.Content.RootDirectory = "Content";

            // The default behavior is to use the system cursor. If you want to 
            // use software cursor implemented by the engine, you would turn 
            // this off. Then provide a initializer for cursor object in the 
            // engine graphics class.
            this.IsMouseVisible = true;
        }

        #endregion

        #region Component Service Provider

        public static IMMEngineService Service { get; private set; }

        #endregion

        #region Operations

        public void Restart()
        {
            // Save immediately because the Exit is an asynchronous call, 
            // which may not finished before Process.Start() is called
            this.Interop.Save?.Save();

            this.Exit();

            using (var p = Process.GetCurrentProcess())
            {
                Process.Start(Assembly.GetEntryAssembly().Location, p.StartInfo.Arguments);
            }
        }

        #endregion

        #region Initialization

        protected override void Initialize()
        {
            // Provide global service access before component initialization
            Service = new MMEngineService(
                new MMEngineAudioService(this.Audio),
                new MMEngineDebugService(this.Debug), 
                new MMEngineGraphicsService(this.Graphics),
                new MMEngineInputService(this.Input),
                new MMEngineInteropService(this.Interop),
                new MMEngineNumericalService(this.Numerical));

            // Load engine persistent asset
            this.Interop.Asset.LoadPackage("Engine.Persistent");

            // Graphics has to initialized first
            this.Graphics.Initialize();
            this.Input.Initialize();
            this.Interop.Initialize();
            this.Numerical.Initialize();

            base.Initialize();
        }

        #endregion

        #region Components

        public IMMEngineAudio Audio
        {
            get { return this.audio ?? (this.audio = new MMEngineNullAudio()); }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                this.audio = value;
            }
        }

        public IMMEngineDebug Debug
        {
            get { return this.debug ?? (this.debug = new MMEngineNullDebug()); }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                this.debug = value;
            }
        }

        public IMMEngineGraphics Graphics
        {
            get { return this.graphics ?? (this.graphics = new MMEngineNullGraphics(this)); }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                this.graphics = value;
            }
        }

        public IMMEngineNumerical Numerical
        {
            get
            {
                return this.numerical
                       ?? (this.numerical = new MMEngineNumerical());
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                this.numerical = value;
            }
        }

        public IMMEngineInterop Interop
        {
            get
            {
                return this.interop
                       ?? (this.interop = new MMEngineNullInterop());
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                this.interop = value;
            }
        }

        public IMMEngineInput Input
        {
            get { return this.input ?? (this.input = new MMEngineNullInput(this)); }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                this.input = value;
            }
        }

        #endregion

        #region Load and Unload

        #endregion

        #region Draw and Update  

        protected override void Update(GameTime time)
        {
            this.UpdateInput(time);

            // You need to call normal Update method in components manually, so
            // that you could control the way they are updating. The usually 
            // things may include:
            //
            // 1. Engine Numerical Component
            // 2. Engine Save Manager
            // 3. Engine Screen Manager
            // 4. Engine Cursor Device
            // 5. Engine Audio Component

            base.Update(time);
        }

        private void UpdateInput(GameTime gameTime)
        {
            // Update input buffer
            this.Input.UpdateInput(gameTime);
        }

        protected override void Draw(GameTime time)
        {
            this.GraphicsDevice.Clear(Color.Transparent);
            this.GraphicsDevice.Clear(ClearOptions.DepthBuffer, new Vector4(0, 0, 0, 0), 1f, 0);
            base.Draw(time);
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            this.Interop.OnExit();
            base.OnExiting(sender, args);

            this.Dispose(true);
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
                        // Don't set camel case properties to null, for there is null checking in property 
                        // injection
                        this.Interop?.Dispose();
                        this.interop = null;

                        this.Input?.Dispose();
                        this.input = null;

                        this.Graphics?.Dispose();
                        this.graphics = null;
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
