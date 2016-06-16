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
            GC.SuppressFinalize(this);
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
                        this.NormalSelect = null;

                        this.HelpSelect?.Dispose();
                        this.HelpSelect = null;

                        this.WorkingInBackground?.Dispose();
                        this.WorkingInBackground = null;

                        this.Busy?.Dispose();
                        this.Busy = null;

                        this.PrecisionSelect?.Dispose();
                        this.PrecisionSelect = null;

                        this.TextSelect?.Dispose();
                        this.TextSelect = null;

                        this.Handwriting?.Dispose();
                        this.Handwriting = null;

                        this.Unavailable?.Dispose();
                        this.Unavailable = null;

                        this.VerticalResize?.Dispose();
                        this.VerticalResize = null;
                        
                        this.HorizontalResize?.Dispose();
                        this.HorizontalResize = null;

                        this.DiagonalResize1?.Dispose();
                        this.DiagonalResize1 = null;

                        this.DiagonalResize2?.Dispose();
                        this.DiagonalResize2 = null;

                        this.Move?.Dispose();
                        this.Move = null;

                        this.AlternativeSelect?.Dispose();
                        this.AlternativeSelect = null;

                        this.LinkSelect?.Dispose();
                        this.LinkSelect = null;
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
