namespace MindEngine.Core.Service
{
    public interface IMMEngineService
    {
        IMMEngineAudioService Audio { get; }

        IMMEngineDebugService Debug { get; }

        IMMEngineInputService Input { get; }

        IMMEngineInteropService Interop { get; }

        IMMEngineNumericalService Numerical { get; }

        IMMEngineGraphicsService Graphics { get; }
    }
}