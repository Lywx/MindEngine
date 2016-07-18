namespace MindEngine.Core.Content.Cursor
{
    using System.Collections.Generic;
    using Component;

    public class MMCursorManager : MMCompositeComponent, IMMCursorManager
    {
        #region Font 

        public MMCursorAsset this[string index] => this.Cursors[index];

        private Dictionary<string, MMCursorAsset> Cursors { get; set; } = new Dictionary<string, MMCursorAsset>();

        #endregion

        #region Constructors

        public MMCursorManager(MMEngine engine)
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
            this.DisposeCursors();
        }

        #endregion Load and Unload

        #region Operations

        public void Add(MMCursorAsset cursorAsset)
        {
            if (!this.Cursors.ContainsKey(cursorAsset.Name))
            {
                this.Cursors.Add(cursorAsset.Name, cursorAsset);
            }
        }

        public void Remove(MMCursorAsset cursorAsset)
        {
            if (this.Cursors.ContainsKey(cursorAsset.Name))
            {
                this.Cursors.Remove(cursorAsset.Name);
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

        private void DisposeCursors()
        {
            foreach (var cursor in this.Cursors.Values)
            {
                cursor.Dispose();
            }

            this.Cursors.Clear();
        }

        #endregion
    }
}