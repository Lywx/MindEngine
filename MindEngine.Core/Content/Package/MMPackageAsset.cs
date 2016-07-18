namespace MindEngine.Core.Content.Package
{
    using System;
    using System.Collections.Generic;
    using Asset;
    using Cursor;
    using Font;
    using Texture;

    /// <summary>
    ///     This is the package that serves as a collection of XML configuration. It
    ///     loaded a unit of gameplay session into asset manager to provide support
    ///     for asset lookup and finding.
    /// </summary>
    public class MMPackageAsset : MMAsset
    {
        #region Constructors and Finalizer

        public MMPackageAsset(string name, string asset)
            : base(name, asset) {}

        ~MMPackageAsset()
        {
            this.Dispose(true);
        }

        #endregion

        public Dictionary<string, MMFontAsset> Fonts { get; } =
            new Dictionary<string, MMFontAsset>();

        public Dictionary<string, MMImageAsset> Texture { get; } =
            new Dictionary<string, MMImageAsset>();

        public Dictionary<string, MMCursorAsset> Cursors { get; } = new Dictionary<string, MMCursorAsset>();

        public void Add(MMFontAsset font)
        {
            this.Fonts.Add(font.Name, font);
        }

        public void Add(MMImageAsset image)
        {
            this.Texture.Add(image.Name, image);
        }

        public void Add(MMCursorAsset cursor)
        {
            this.Cursors.Add(cursor.Name, cursor);
        }

        #region IDisposable

        private bool IsDisposed { get; set; }

        public override void Dispose()
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
                        foreach (var asset in this.Fonts.Values)
                        {
                            asset.Dispose();
                        }
                        this.Fonts.Clear();

                        foreach (var asset in this.Texture.Values)
                        {
                            asset.Dispose();
                        }
                        this.Texture.Clear();

                        foreach (var asset in this.Cursors.Values)
                        {
                            asset.Dispose();
                        }
                        this.Cursors.Clear();
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
