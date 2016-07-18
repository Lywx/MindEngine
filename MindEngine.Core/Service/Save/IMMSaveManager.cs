namespace MindEngine.Core.Service.Save
{
    using Component;

    public interface IMMSaveManager : IMMGameComponent
    {
        void Save();

        void Load();
    }
}