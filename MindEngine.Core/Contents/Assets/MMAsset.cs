namespace MindEngine.Core.Contents.Assets
{
    using System;

    public abstract class MMAsset
    {
        #region Constructors 

        protected MMAsset(string name, string path)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            this.Name = name;
            this.Path = path;
        }

        #endregion

        public bool Archive { get; protected set; } = false;

        /// <summary>
        /// The asset file path.
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// The descriptive name for the asset. For example, fonts will have names
        /// like "Lucida Console Regular". This should be differentiated between
        /// the asset filename.
        /// </summary>
        public string Name { get; set; }
    }
}