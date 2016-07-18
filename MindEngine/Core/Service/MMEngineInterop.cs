namespace MindEngine.Core.Service
{
    using Component;
    using Content.Asset;
    using Event;
    using IO.Directory;
    using Process;
    using Save;
    using State;

    public class MMEngineInterop : MMCompositeComponent, IMMEngineInterop
    {
        #region Constructors and Finalizer

        public MMEngineInterop(MMEngine engine)
            : base(engine)
        {
            this.Asset   = new MMAssetManager(engine);
            this.File    = new MMDirectoryManager();
            this.Event   = new MMEventManager(engine);
            this.Game    = new MMGameManager(engine);
            this.Process = new MMProcessManager(engine);
            this.Screen  = new MMScreenManager(engine);
        }

        #endregion

        public MMAssetManager Asset { get; private set; }

        public MMDirectoryManager File { get; private set; }

        public MMEventManager Event { get; private set; }

        public new MMGameManager Game { get; private set; }

        public MMProcessManager Process { get; private set; }

        public MMScreenManager Screen { get; private set; }

        public MMSaveManager Save { get; set; }

        #region Initialization

        public override void Initialize()
        {
            this.Asset.Initialize();
        }

        #endregion

        #region Exit

        public void OnExit()
        {
            this.Screen.OnExit();
            this.Game.OnExit();
        }

        #endregion

        #region IDisposable

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Asset?.Dispose();
                this.Asset = null;

                //TODO(Wuxiang)
                //this.Console?.Dispose();
                //this.Console = null;

                this.Engine = null;

                this.File?.Dispose();
                this.File = null;

                this.Event?.Dispose();
                this.Event = null;

                this.Game?.Dispose();
                this.Game = null;

                this.Process?.Dispose();
                this.Process = null;

                this.Save?.Dispose();
                this.Save = null;

                this.Screen?.Dispose();
                this.Screen = null;
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
