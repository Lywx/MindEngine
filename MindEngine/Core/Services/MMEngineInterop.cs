namespace MindEngine.Core.Services
{
    using Components;
    using Contents.Assets;
    using Events;
    using IO.Directory;
    using Microsoft.Xna.Framework;
    using Processes;
    using Saves;

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

            //this.Screen = screen;
            //this.Engine.Components.Add(this.Screen);

            //this.Console = console;
            //this.Engine.Components.Add(this.Console);
        }

        #endregion

        public IMMAssetManager Asset { get; private set; }

        //TODO(Wuxiang)
        //public MMConsole Console { get; set; }

        public IMMDirectoryManager File { get; private set; }

        public IMMEventManager Event { get; private set; }

        public new IMMGameManager Game { get; private set; }

        public IMMProcessManager Process { get; private set; }

        //TODO(Wuxiang)
        //public IMMScreenDirector Screen { get; private set; }

        public IMMSaveManager Save { get; set; }

        #region Initialization

        public override void Initialize()
        {
            this.Asset.Initialize();
        }

        #endregion

        #region Exit

        public void OnExit()
        {
            //TODO(Wuxiang)
            //this.Screen.OnExit();
            this.Game.OnExit();
        }

        #endregion

        #region Update

        public override void UpdateInput(GameTime time)
        {
            //TODO(Wuxiang)
            //this.Screen.UpdateInput(time);
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

                //TODO(Wuxiang)
                //this.Screen?.Dispose();
                //this.Screen = null;
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
