namespace MindEngine.Core.Contents.Cursors
{
    using System;
    using Microsoft.Xna.Framework.Graphics;

    public class MMCursorResource : IDisposable
    {
        public Texture2D NormalSelect { get; set; }

        public Texture2D HelpSelect { get; set; }

        public Texture2D WorkingInBackground { get; set; }

        public Texture2D Busy { get; set; }

        public Texture2D PrecisionSelect { get; set; }

        public Texture2D TextSelect { get; set; }

        public Texture2D Handwriting { get; set; }

        public Texture2D Unavailable { get; set; }

        public Texture2D VerticalResize { get; set; }

        public Texture2D HorizontalResize { get; set; }

        public Texture2D DiagonalResize1 { get; set; }

        public Texture2D DiagonalResize2 { get; set; }

        public Texture2D Move { get; set; }

        public Texture2D AlternativeSelect { get; set; }

        public Texture2D LinkSelect { get; set; }

        #region IDisposable

        private bool IsDisposed { get; set; }

        public void Dispose()
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
                        this.NormalSelect?.Dispose();
                        this.HelpSelect?.Dispose();
                        this.WorkingInBackground?.Dispose();
                        this.Busy?.Dispose();
                        this.PrecisionSelect?.Dispose();
                        this.TextSelect?.Dispose();
                        this.Handwriting?.Dispose();
                        this.Unavailable?.Dispose();
                        this.VerticalResize?.Dispose();
                        this.HorizontalResize?.Dispose();
                        this.DiagonalResize1?.Dispose();
                        this.DiagonalResize2?.Dispose();
                        this.Move?.Dispose();
                        this.AlternativeSelect?.Dispose();
                        this.LinkSelect?.Dispose();
                    }

                    this.IsDisposed = true;
                }
            }
            catch
            {
                // Ignored
            }
            finally {}
        }

        #endregion
    }
}
