namespace MindEngine.Core.Sessions
{
    public interface IMMSession<out TData, out TController>
        where TData : IMMSessionData, new()
        where TController : IMMSessionController<TData>, new()
    {
        TData Data { get; }

        TController Controller { get; }

        void Update();

        void Save();
    }
}
