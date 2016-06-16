namespace MindEngine.Core.Contents.Texture
{
    using System;
    using Microsoft.Xna.Framework.Graphics;

    public class MMImage : IDisposable
    {
        public MMImage(MMImageDesign design, Texture2D resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException(nameof(resource));
            }

            this.Design = design;
            this.Resource = resource;
        }

        public MMImageDesign Design { get; private set; }

        public Texture2D Resource { get; private set; }


        #region IDisposable

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool IsDisposed { get; set; }

        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (!this.IsDisposed)
                    {
                        this.Resource?.Dispose();
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