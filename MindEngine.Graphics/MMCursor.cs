namespace MindEngine.Graphics
{
    using System.Windows.Forms;

    /// <summary>
    ///     A basic software cursor.
    /// </summary>
    public class MMCursor
    {
        public MMCursor(string cursorDir)
        {
            this.CursorDir = cursorDir;
        }

        #region Basic Setup

        private string CursorDir { get; set; }

        #endregion

        #region Additional Setup

        public string FileNormalSelect        { get; set; }

        public string FileHelpSelect          { get; set; }

        public string FileWorkingInBackground { get; set; }

        public string FileBusy                { get; set; }

        public string FilePrecisionSelect     { get; set; }

        public string FileTextSelect          { get; set; }

        public string FileHandwriting         { get; set; }

        public string FileUnavailable         { get; set; }

        public string FileVerticalResize      { get; set; }

        public string FileHorizontalResize    { get; set; }

        public string FileDiagonalResize1     { get; set; }

        public string FileDiagonalResize2     { get; set; }

        public string FileMove                { get; set; }

        public string FileAlternativeSelect   { get; set; }

        public string FileLinkSelect          { get; set; }

        #endregion

        #region Operations

        public void SetNormalSelect        () => this.Set(this.FileNormalSelect);

        public void SetHelpSelect          () => this.Set(this.FileHelpSelect);
        
        public void SetWorkingInBackground () => this.Set(this.FileWorkingInBackground);
        
        public void SetBusy                () => this.Set(this.FileBusy);
        
        public void SetPrecisionSelect     () => this.Set(this.FilePrecisionSelect);

        public void SetTextSelect          () => this.Set(this.FileTextSelect);

        public void SetHandwriting         () => this.Set(this.FileHandwriting);

        public void SetUnavailable         () => this.Set(this.FileUnavailable);

        public void SetVerticalResize      () => this.Set(this.FileVerticalResize);

        public void SetHorizontalResize    () => this.Set(this.FileHorizontalResize);

        public void SetDiagonalResize1     () => this.Set(this.FileDiagonalResize1);

        public void SetDiagonalResize2     () => this.Set(this.FileDiagonalResize2);

        public void SetMove                () => this.Set(this.FileMove);

        public void SetAlternativeSelect   () => this.Set(this.FileAlternativeSelect);

        public void SetLinkSelect          () => this.Set(this.FileLinkSelect);

        private void Set(string cursorFile)
        {
            // Skip empty path
            if (string.IsNullOrEmpty(cursorFile))
            {
                return;
            }

            Cursor.Current = new Cursor(this.CursorDir + cursorFile);
        }

        #endregion
    }
}
