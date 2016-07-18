namespace MindEngine.Graphics
{
    using System;
    using Core;
    using Core.Content.Cursor;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class MMCursorDevice : MMObject
    {
        public MMCursorDevice(MMCursorAsset asset)
        {
            if (asset == null)
            {
                throw new ArgumentNullException(nameof(asset));
            }

            this.Resource = asset.Resource;
            this.Hotpot   = asset.Hotspot;

            this.TextureCurrent = asset.Resource.NormalSelect;
            this.HotpotCurrent = asset.Hotspot.NormalSelect;
        }

        ~MMCursorDevice()
        {
            this.Dispose(true);
        }

        private Vector2 Position { get; set; }

        private MMCursorResource Resource { get; }

        private MMCursorHotspot Hotpot { get; }

        private Texture2D TextureCurrent { get; set; }

        private Vector2 HotpotCurrent { get; set; }

        public void Update(GameTime time)
        {
            this.Position = this.EngineInput.Mouse.Position.ToVector2();
        }

        public void Draw(GameTime time)
        {
            this.EngineRenderer.Draw(this.TextureCurrent, this.Position - this.HotpotCurrent, 0f);
        }

        #region Operations

        private void Set(MMCursorShape shape)
        {
            switch (shape)
            {
                case MMCursorShape.NormalSelect:
                    this.TextureCurrent = this.Resource.NormalSelect;
                    this.HotpotCurrent = this.Hotpot.NormalSelect;
                    break;
                case MMCursorShape.HelpSelect:
                    this.TextureCurrent = this.Resource.HelpSelect;
                    this.HotpotCurrent = this.Hotpot.HelpSelect;
                    break;
                case MMCursorShape.WorkingInBackground:
                    this.TextureCurrent = this.Resource.WorkingInBackground;
                    this.HotpotCurrent = this.Hotpot.WorkingInBackground;
                    break;
                case MMCursorShape.Busy:
                    this.TextureCurrent = this.Resource.Busy;
                    this.HotpotCurrent = this.Hotpot.Busy;
                    break;
                case MMCursorShape.PrecisionSelect:
                    this.TextureCurrent = this.Resource.PrecisionSelect;
                    this.HotpotCurrent = this.Hotpot.PrecisionSelect;
                    break;
                case MMCursorShape.TextSelect:
                    this.TextureCurrent = this.Resource.TextSelect;
                    this.HotpotCurrent = this.Hotpot.TextSelect;
                    break;
                case MMCursorShape.Handwriting:
                    this.TextureCurrent = this.Resource.Handwriting;
                    this.HotpotCurrent = this.Hotpot.Handwriting;
                    break;
                case MMCursorShape.Unavailable:
                    this.TextureCurrent = this.Resource.Unavailable;
                    this.HotpotCurrent = this.Hotpot.Unavailable;
                    break;
                case MMCursorShape.VerticalResize:
                    this.TextureCurrent = this.Resource.VerticalResize;
                    this.HotpotCurrent = this.Hotpot.VerticalResize;
                    break;
                case MMCursorShape.HorizontalResize:
                    this.TextureCurrent = this.Resource.HorizontalResize;
                    this.HotpotCurrent = this.Hotpot.HorizontalResize;
                    break;
                case MMCursorShape.DiagonalResize1:
                    this.TextureCurrent = this.Resource.DiagonalResize1;
                    this.HotpotCurrent = this.Hotpot.DiagonalResize1;
                    break;
                case MMCursorShape.DiagonalResize2:
                    this.TextureCurrent = this.Resource.DiagonalResize2;
                    this.HotpotCurrent = this.Hotpot.DiagonalResize2;
                    break;
                case MMCursorShape.Move:
                    this.TextureCurrent = this.Resource.Move;
                    this.HotpotCurrent = this.Hotpot.Move;
                    break;
                case MMCursorShape.AlternativeSelect:
                    this.TextureCurrent = this.Resource.AlternativeSelect;
                    this.HotpotCurrent = this.Hotpot.AlternativeSelect;
                    break;
                case MMCursorShape.LinkSelect:
                    this.TextureCurrent = this.Resource.LinkSelect;
                    this.HotpotCurrent = this.Hotpot.LinkSelect;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(shape), shape, null);
            }
        }

        #endregion

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
                        this.Resource.Dispose();
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
