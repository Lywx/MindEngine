namespace MindEngine.Core.Service
{
    using Component;

    public interface IMMEngineInterop : IMMGameComponent, IMMEngineInteropService, IMMUpdateableOperations
    {
        void OnExit();
    }
}
