namespace MindEngine.Core.Services
{
    public interface IMMEngineService
    {
        IMMEngineAudioService Audio { get; }

        IMMEngineInputService Input { get; }

        IMMEngineInteropService Interop { get; }

        IMMEngineNumericalService Numerical { get; }

        IMMEngineGraphicsService Graphics { get; }
    }
}