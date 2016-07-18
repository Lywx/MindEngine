namespace MindEngine.Core.Session
{
    using Component;

    /// <summary>
    ///     Session controller is the main control for session management. It should
    ///     handle the communication with game engine and more advanced operations.
    /// </summary>
    public interface IMMSessionManager<TData> : IMMGameComponent
    {
    }
}
