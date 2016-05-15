namespace MindEngine.Core.Contents.Texture
{
    using Components;

    public interface IMMTextureManager : IMMGameComponent
    {
        MMImage this[string index] { get; }

        void Add(MMImageAsset imageAsset);

        void Remove(MMImageAsset imageAsset);
    }
}