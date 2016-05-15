namespace MindEngine.Core.Services
{
    using Contents.Assets;
    using Microsoft.Xna.Framework.Content;

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

        public IMMAssetManager Asset => this.interop.Asset;

        public ContentManager Content => this.interop.Content;

        public MMConsole Console
        {
            get
            {
                return this.interop.Console;
            }
            set
            {
                this.interop.Console = value;
            }
        }

        public IMMDirectoryManager File => this.interop.File;

        public MMEngine Engine => this.interop.Engine;

        public IMMEventManager Event => this.interop.Event;

        public IMMGameManager Game => this.interop.Game;

        public IMMProcessManager Process => this.interop.Process;

        public IMMScreenDirector Screen => this.interop.Screen;

        public IMMSaveManager Save
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