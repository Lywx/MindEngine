namespace MindEngine.Core.Contents.Assets
{
    using Components;
    using Cursors;
    using Fonts;
    using Texture;

    public interface IMMAssetManager : IMMGameComponent
    {
        #region Content

        IMMFontManager Fonts { get; }

        IMMTextureManager Texture { get; }

        IMMCursorManager Cursors { get; }

        #endregion

        #region Operations

        void LoadPackage(string packageName, bool async = false);

        void UnloadPackage(string packageName);

        #endregion
    }
}
