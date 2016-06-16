namespace MindEngine.Core.Services
{
    using Components;

    public interface IMMEngineInterop : IMMGameComponent, IMMEngineInteropService, IMMUpdateableOperations
    {
        void OnExit();
    }
}
