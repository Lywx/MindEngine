namespace MindEngine.Input
{
    using System;

    public struct MMInputAction
    {
        /// <summary>
        /// Default initialized structure.
        /// </summary>
        public static MMInputAction None = Guid.Empty;

        #region Constructors

        public MMInputAction(Guid guid)
        {
            this.Guid = guid;
        }

        #endregion

        private Guid Guid { get; }

        public static implicit operator MMInputAction(Guid guid)
        {
            return new MMInputAction(guid);
        }
    }
}