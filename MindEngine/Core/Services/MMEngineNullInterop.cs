namespace MindEngine.Core.Services
{
    using Contents.Assets;
    using Event;
    using IO.Directory;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Process;
    using Save;
    using Scenes;

    internal class MMEngineNullInterop : IMMEngineInterop
    {
        public ContentManager Content { get; }

        public void Initialize() {}

        public void Dispose() {}

        public MMAssetManager Asset { get; }

        public MMDirectoryManager File { get; }

        public MMEventManager Event { get; }

        public MMGameManager Game { get; }

        public MMEngine Engine { get; }

        public MMProcessManager Process { get; }

        public MMScreenManager Screen { get; }

        public MMSaveManager Save { get; set; }

        public void OnExit() {}

        public void Update(GameTime time) {}
    }
}
