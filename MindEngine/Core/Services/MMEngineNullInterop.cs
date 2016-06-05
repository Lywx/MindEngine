namespace MindEngine.Core.Services
{
    using Contents.Assets;
    using IO.Directory;
    using Microsoft.Xna.Framework.Content;

    internal class MMEngineNullInterop : IMMEngineInterop
    {
        public void Initialize() {}

        public void Dispose() {}

        public IMMAssetManager Asset { get; }

        public ContentManager Content { get; }

        public IMMDirectoryManager File { get; }

        public IMMGameManager Game { get; }

        public MMEngine Engine { get; }

        public void OnExit() {}
    }
}
