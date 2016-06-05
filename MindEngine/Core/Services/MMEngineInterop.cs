namespace MindEngine.Core.Services
{
    using Components;
    using Contents.Assets;
    using IO.Directory;
    using Microsoft.Xna.Framework;

    public class MMEngineInterop : MMCompositeComponent, IMMEngineInterop
    {
        #region Constructors and Finalizer

        public MMEngineInterop(MMEngine engine)
            : base(engine)
        {
            this.Asset = new MMAssetManager(engine);
            this.Engine.Components.Add(this.Asset);

            this.File = new MMDirectoryManager();

            //TODO(Wuxiang)
            //this.Event = new MMEventManager(engine)
            //{
            //    UpdateOrder = 3
            //};
            //this.Engine.Components.Add(this.Event);

            this.Game = new MMGameManager(engine);

            //TODO(Wuxiang)
            //this.Process = new MMProcessManager(engine)
            //{
            //    UpdateOrder = 4
            //};
            //this.Engine.Components.Add(this.Process);

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

        //TODO(Wuxiang)
        //public IMMEventManager Event { get; private set; }

        public new IMMGameManager Game { get; private set; }

        //TODO(Wuxiang)

        //public IMMProcessManager Process { get; private set; }

        //TODO(Wuxiang)
        //public IMMScreenDirector Screen { get; private set; }

        //TODO(Wuxiang)
        //public IMMSaveManager Save { get; set; }

        #region Initialization

        public override void Initialize() {}

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

                //TODO(Wuxiang)
                //this.Event?.Dispose();
                //this.Event = null;

                this.Game?.Dispose();
                this.Game = null;

                //TODO(Wuxiang)
                //this.Process?.Dispose();
                //this.Process = null;

                //this.Save?.Dispose();
                //this.Save = null;

                //this.Screen?.Dispose();
                //this.Screen = null;
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
