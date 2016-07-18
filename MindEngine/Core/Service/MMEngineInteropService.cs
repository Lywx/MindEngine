namespace MindEngine.Core.Service
{
    using Content.Asset;
    using Event;
    using IO.Directory;
    using Process;
    using Save;
    using State;

    /// <remarks>
    ///     Sealed for added new keyword and changed accessibility in MMEngine property
    /// </remarks>
    public sealed class MMEngineInteropService : IMMEngineInteropService
    {
        private IMMEngineInterop Interop { get; }

        public MMEngineInteropService(IMMEngineInterop interop)
        {
            this.Interop = interop;
        }

        public MMAssetManager Asset => this.Interop.Asset;

        //TODO(Wuxiang)
        //public MMConsole Console
        //{
        //    get
        //    {
        //        return this.interop.Console;
        //    }
        //    set
        //    {
        //        this.interop.Console = value;
        //    }
        //}

        public MMDirectoryManager File => this.Interop.File;

        public MMEngine Engine => this.Interop.Engine;

        public MMEventManager Event => this.Interop.Event;

        public MMGameManager Game => this.Interop.Game;

        public MMProcessManager Process => this.Interop.Process;

        public MMScreenManager Screen => this.Interop.Screen;

        public MMSaveManager Save
        {
            get
            {
                return this.Interop.Save;
            }

            set
            {
                this.Interop.Save = value;
            }
        }
    }
}