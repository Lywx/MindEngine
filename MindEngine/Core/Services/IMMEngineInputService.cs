namespace MindEngine.Core.Services
{
    using Input;

    public interface IMMEngineInputService
    {
        IMMInputEvent Event { get; }

        IMMInputState State { get; }
    }
}
