namespace MindEngine.Core.Contents.Cursors
{
    using Components;

    public interface IMMCursorManager : IMMGameComponent
    {
        MMCursorAsset this[string index] { get; }

        void Add(MMCursorAsset cursorAsset);

        void Remove(MMCursorAsset cursorAsset);
    }
}