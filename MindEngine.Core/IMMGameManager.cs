namespace MindEngine.Core
{
    using Components;

    public interface IMMGameManager : IMMGameComponent
    {
        IMMGame Game { get; }

        void Add(IMMGame game);

        void OnExit();
    }
}