namespace MindEngine.Core.Content.Font
{
    using Component;

    public interface IMMFontManager : IMMGameComponent
    {
        #region Font Access

        MMFont this[string index] { get; }

        #endregion

        #region Font Loading

        /// <summary>
        /// This method should be called from asset manager.
        /// </summary>
        /// <param name="fontAsset"></param>
        void Add(MMFontAsset fontAsset);

        #endregion

        void Remove(MMFontAsset fontAsset);
    }
}