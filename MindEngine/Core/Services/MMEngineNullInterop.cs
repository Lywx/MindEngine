namespace MindEngine.Core.Services
{
    using Contents.Assets;
    using Events;
    using IO.Directory;
    using Microsoft.Xna.Framework.Content;
    using Processes;
    using Saves;

    internal class MMEngineNullInterop : IMMEngineInterop
    {
        public void Initialize() {}

        public void Dispose() {}

        public IMMAssetManager Asset { get; }

        public ContentManager Content { get; }

        public IMMDirectoryManager File { get; }

        public IMMEventManager Event { get; }

        public IMMGameManager Game { get; }

        public MMEngine Engine { get; }

        public IMMProcessManager Process { get; }

        public IMMSaveManager Save { get; set; }

        public void OnExit() {}
    }
}
