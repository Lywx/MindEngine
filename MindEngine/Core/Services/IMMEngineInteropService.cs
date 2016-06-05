namespace MindEngine.Core.Services
{
    using Contents.Assets;
    using IO.Directory;

    public interface IMMEngineInteropService 
    {
        IMMAssetManager Asset { get; }

        // TODO(Wuxiang)
        //IMMConsole Console { get; set; }

        IMMDirectoryManager File { get; }

        // TODO(Wuxiang)
        //IMMEventManager Event { get; }

        IMMGameManager Game { get; }

        MMEngine Engine { get; }

        // TODO(Wuxiang)
        //IMMProcessManager Process { get; }

        // TODO(Wuxiang)
        //IMMScreenDirector Screen { get; }

        /// <remarks>
        /// Save that is replaceable in specific game 
        /// </remarks>
        //IMMSaveManager Save { get; set; }
    }
}