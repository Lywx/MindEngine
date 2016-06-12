namespace MindEngine.Core.Contents.Cursors
{
    using System;
    using Assets;

    public class MMCursorAsset : MMAsset
    {
        public MMCursorAsset(string name, MMCursorDesign design, MMCursorHotspot hotspot) : base(name, string.Empty)
        {
            if (design == null)
            {
                throw new ArgumentNullException(nameof(design));
            }

            if (hotspot == null)
            {
                throw new ArgumentNullException(nameof(hotspot));
            }

            this.Design  = design;
            this.Hotspot = hotspot;
        }

        public MMCursorDesign Design { get; }

        public MMCursorHotspot Hotspot { get; }

        public MMCursorResource Resource { get; set; } = new MMCursorResource();

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
