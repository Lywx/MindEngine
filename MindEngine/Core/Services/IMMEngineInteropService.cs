namespace MindEngine.Core.Services
{
    using Contents.Assets;
    using Event;
    using IO.Directory;
    using Process;
    using Save;
    using Scenes;

    public interface IMMEngineInteropService
    {
        MMAssetManager Asset { get; }

        MMDirectoryManager File { get; }

        MMEventManager Event { get; }

        MMGameManager Game { get; }

        MMEngine Engine { get; }

        MMProcessManager Process { get; }

        MMScreenManager Screen { get; }

        /// <remarks>
        ///     Save that is replaceable in specific game
        /// </remarks>
        MMSaveManager Save { get; set; }
    }
}
