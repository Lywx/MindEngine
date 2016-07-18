namespace MindEngine.Core.Content.Asset
{
    using Component;
    using Cursor;
    using Font;
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
