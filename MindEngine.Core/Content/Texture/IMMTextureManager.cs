namespace MindEngine.Core.Content.Texture
{
    using Component;

    public interface IMMTextureManager : IMMGameComponent
    {
        MMImage this[string index] { get; }

        void Add(MMImageAsset imageAsset);

        void Remove(MMImageAsset imageAsset);
    }
}