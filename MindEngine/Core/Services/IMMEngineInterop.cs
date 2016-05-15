namespace MindEngine.Core.Services
{
    using Components;

    public interface IMMEngineInterop : IMMCompositeComponent, IMMEngineInteropService
    {
        void OnExit();
    }
}
