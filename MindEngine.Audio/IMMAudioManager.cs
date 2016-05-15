namespace MindEngine.Audio
{
    public interface IMMAudioManager
    {
        #region Audio Access

        MMAudio this[string index] { get; }

        #endregion

        #region Audio Loading

        /// <summary>
        /// This method should be called from asset manager.
        /// </summary>
        /// <param name="audioAsset"></param>
        void Add(MMAudioAsset audioAsset);

        #endregion

        void Remove(MMAudioAsset audioAsset);
    }
}