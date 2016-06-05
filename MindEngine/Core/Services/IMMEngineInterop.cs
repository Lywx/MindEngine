namespace MindEngine.Core.Services
{
    using Components;

    public interface IMMEngineInterop : IMMGameComponent, IMMEngineInteropService
    {
        void OnExit();
    }
}
