namespace MindEngine.Core.Services
{
    using Contents.Assets;
    using IO.Directory;
    using Microsoft.Xna.Framework.Content;

    public interface IMMEngineInteropService
    {
        IMMAssetManager Asset { get; }

        ContentManager Content { get; }

        MMConsole Console { get; set; }

        IMMDirectoryManager File { get; }

        IMMEventManager Event { get; }

        IMMGameManager Game { get; }

        MMEngine Engine { get; }

        IMMProcessManager Process { get; }

        IMMScreenDirector Screen { get; }

        /// <remarks>
        /// Save that is replaceable in specific game 
        /// </remarks>
        IMMSaveManager Save { get; set; }
    }
}