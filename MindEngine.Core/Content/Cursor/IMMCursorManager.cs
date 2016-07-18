namespace MindEngine.Core.Content.Cursor
{
    using Component;

    public interface IMMCursorManager : IMMGameComponent
    {
        MMCursorAsset this[string index] { get; }

        void Add(MMCursorAsset cursorAsset);

        void Remove(MMCursorAsset cursorAsset);
    }
}