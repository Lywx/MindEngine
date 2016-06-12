namespace MindEngine.Core.Contents.Assets
{
    using System;

    public abstract class MMAsset : IDisposable
    {
        #region Constructors and Finalizer

        protected MMAsset(string name, string asset)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (asset == null)
            {
                throw new ArgumentNullException(nameof(asset));
            }

            this.Name = name;
            this.Asset = asset;
        }

        #endregion

        public bool Archive { get; protected set; } = false;

        /// <summary>
        /// The asset file path.
        /// </summary>
        public string Asset { get; }

        /// <summary>
        /// The descriptive name for the asset. For example, fonts will have names
        /// like "Lucida Console Regular". This should be differentiated between
        /// the asset filename.
        /// </summary>
        public string Name { get; set; }

        public abstract void Dispose();
    }
}