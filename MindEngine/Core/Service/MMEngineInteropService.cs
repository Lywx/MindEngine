namespace MindEngine.Core.Service
{
    using Content.Asset;
    using Event;
    using IO.Directory;
    using Process;
    using Save;
    using Scripting;
    using State;

    /// <summary>
    /// Provide a wrapper around the service in order to hot swap the core 
    /// module in engine.
    /// </summary>
    public sealed class MMEngineInteropService : IMMEngineInteropService
    {
        private IMMEngineInterop Interop { get; }

        public MMEngineInteropService(IMMEngineInterop interop)
        {
            this.Interop = interop;
        }

        public MMAssetManager Asset => this.Interop.Asset;

        public MMDirectoryManager File => this.Interop.File;

        public MMEngine Engine => this.Interop.Engine;

        public IMMScriptManager Script
        {
            get { return this.Interop.Script; }
            set { this.Interop.Script = value; }
        }

        public MMEventManager Event => this.Interop.Event;

        public MMGameManager Game => this.Interop.Game;

        public MMProcessManager Process => this.Interop.Process;

        public MMScreenManager Screen => this.Interop.Screen;

        public MMSaveManager Save
        {
            get { return this.Interop.Save; } 
            set { this.Interop.Save = value; }
        }
    }
}