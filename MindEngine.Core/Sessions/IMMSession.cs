namespace MindEngine.Core.Sessions
{
    public interface IMMSession<out TData>
        where TData : IMMSessionData, new()
    {
        TData Data { get; }

        void Initialize();

        void Update();

        void Save();
    }
}
