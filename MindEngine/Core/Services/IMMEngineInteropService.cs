namespace MindEngine.Core.Services
{
    using Contents.Assets;
    using Events;
    using IO.Directory;
    using Processes;
    using Saves;

    public interface IMMEngineInteropService 
    {
        IMMAssetManager Asset { get; }

        // TODO(Wuxiang)
        //IMMConsole Console { get; set; }

        IMMDirectoryManager File { get; }

        IMMEventManager Event { get; }

        IMMGameManager Game { get; }

        MMEngine Engine { get; }

        IMMProcessManager Process { get; }

        // TODO(Wuxiang)
        //IMMScreenDirector Screen { get; }

        /// <remarks>
        /// Save that is replaceable in specific game 
        /// </remarks>
        IMMSaveManager Save { get; set; }
    }
}