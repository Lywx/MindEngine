namespace MindEngine.Core.Service
{
    using Content.Asset;
    using Event;
    using IO.Directory;
    using Process;
    using Save;
    using State;

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
