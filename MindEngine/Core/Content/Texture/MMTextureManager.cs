namespace MindEngine.Core.Content.Texture
{
    using System.Collections.Generic;
    using Component;

    public class MMTextureManager : MMCompositeComponent, IMMTextureManager
    {
        #region Font 

        public MMImage this[string index] => this.Image[index];

        private Dictionary<string, MMImage> Image { get; set; } = new Dictionary<string, MMImage>();

        #endregion

        #region Constructors

        public MMTextureManager(MMEngine engine)
            : base(engine)
        {
        }

        #endregion

        #region Load and Unload

        protected override void LoadContent()
        {
        }

        /// <summary>
        /// Release all references to the fonts.
        /// </summary>
        protected override void UnloadContent()
        {
            this.DisposeImage();

            this.Image.Clear();
        }

        #endregion Load and Unload

        #region Operations

        public void Add(MMImageAsset imageAsset)
        {
            if (!this.Image.ContainsKey(imageAsset.Name))
            {
                this.Image.Add(imageAsset.Name, imageAsset.ToImage());
            }
        }

        public void Remove(MMImageAsset imageAsset)
        {
            if (this.Image.ContainsKey(imageAsset.Name))
            {
                this.Image.Remove(imageAsset.Name);
            }
        }

        #endregion

        #region IDisposable

        private bool IsDisposed { get; set; }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (!this.IsDisposed)
                    {
                        this.UnloadContent();
                    }

                    this.IsDisposed = true;
                }
            }
            catch
            {
                // Ignored
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        private void DisposeImage()
        {
            foreach (var image in this.Image.Values)
            {
                image.Dispose();
            }
        }

        #endregion
    }
}