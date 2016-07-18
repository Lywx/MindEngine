namespace MindEngine.Core
{
    using Component;

    public interface IMMGameManager : IMMGameComponent
    {
        IMMGame Game { get; }

        void Add(IMMGame game);

        void OnExit();
    }
}