namespace MindEngine.Core.Services.Saves
{
    using Components;

    public interface IMMSaveManager : IMMGameComponent
    {
        void Save();

        void Load();
    }
}