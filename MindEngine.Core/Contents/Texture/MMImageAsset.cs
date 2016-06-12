namespace MindEngine.Core.Contents.Texture
{
    using Assets;
    using Microsoft.Xna.Framework.Graphics;

    public class MMImageAsset : MMAsset
    {
        public MMImageAsset(string name, string asset, MMImageDesign design) 
            : base(name, asset)
        {
            this.Design = design;
        }

        ~MMImageAsset()
        {
            this.Dispose(true);
        }

        #region Resource

        public MMImageDesign Design { get; set; }

        public Texture2D Resource { get; set; }

        #endregion

        #region Conversion

        public MMImage ToImage()
        {
            return new MMImage(this.Design, this.Resource);
        }

        #endregion

        #region IDisposable

        private bool IsDisposed { get; set; }

        public override void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (!this.IsDisposed)
                    {
                        this.Resource.Dispose();
                        this.Resource = null;
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
            }
        }

        #endregion
    }
}
