namespace MindEngine.Core.Services
{
    using Audio;
    using Contents.Assets;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;

    // TODO
    internal class MMEngineNullInterop : IMMEngineInterop
    {
        public MMEngineNullInterop(MMEngine engine) 
            : base(engine)
        {
        }

        public IMMAssetManager Asset { get; }

        public IMMAudioController Audio { get; }

        public ContentManager Content { get; }

        public MMConsole Console { get; set; }

        public IMMDirectoryManager File { get; }

        public IMMEventManager Event { get; }

        public new IMMGameManager Game { get; }

        public IMMProcessManager Process { get; }

        public IMMScreenDirector Screen { get; }

        public IMMSaveManager Save { get; set; }

        public override void Initialize()
        {
        }

        public override void UpdateInput(GameTime time)
        {
        }

        public void OnExit()
        {
        }
    }
}