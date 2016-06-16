namespace MindEngine.Core.Services
{
    using Contents.Assets;
    using Event;
    using IO.Directory;
    using Process;
    using Save;
    using Scenes;

    /// <remarks>
    ///     Sealed for added new keyword and changed accessibility in MMEngine property
    /// </remarks>
    public sealed class MMEngineInteropService : IMMEngineInteropService
    {
        private readonly IMMEngineInterop interop;

        public MMEngineInteropService(IMMEngineInterop interop)
        {
            this.interop = interop;
        }

        public MMAssetManager Asset => this.interop.Asset;

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

        public MMDirectoryManager File => this.interop.File;

        public MMEngine Engine => this.interop.Engine;

        public MMEventManager Event => this.interop.Event;

        public MMGameManager Game => this.interop.Game;

        public MMProcessManager Process => this.interop.Process;

        public MMScreenManager Screen => this.interop.Screen;

        public MMSaveManager Save
        {
            get
            {
                return this.interop.Save;
            }

            set
            {
                this.interop.Save = value;
            }
        }
    }
}