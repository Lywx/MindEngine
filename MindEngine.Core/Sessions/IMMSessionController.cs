namespace MindEngine.Core.Sessions
{
    using Components;

    /// <summary>
    ///     Session controller is the main control for session management. It should
    ///     handle the communication with game engine and more advanced operations.
    /// </summary>
    public interface IMMSessionController<TData> : IMMGameComponent
    {
        TData Data { get; set; }

        void Update();
    }
}
