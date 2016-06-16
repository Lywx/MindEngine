namespace MindEngine.Core.Services.Save
{
    using Components;

    public interface IMMSaveManager : IMMGameComponent
    {
        void Save();

        void Load();
    }
}