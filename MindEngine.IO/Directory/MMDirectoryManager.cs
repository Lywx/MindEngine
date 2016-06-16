namespace MindEngine.IO.Directory
{
    using System;
    using System.IO;
    using Platform;

#if WINDOWS

#endif

    /// <summary>
    /// It handles the basic content searching and maintenance.
    /// </summary>
    public class MMDirectoryManager : IMMDirectoryManager
    {
        private static readonly IMMPlatformPath PlatformPath 
#if WINDOWS
            = new MMPlatformPathWin32();
#endif

        private static string ConfigurationDirectory => PlatformPath.ConfigurationDirectory;

        private static string ContentDirectory => PlatformPath.ContentDirectory;

        private static string DataDirectory => PlatformPath.DataDirectory;

        private static string SaveDirectory => PlatformPath.SaveDirectory;

        #region Constructors

        public MMDirectoryManager()
        {
            this.CreateDirectory();
        }

        #endregion Constructors

        #region Path

        public static string ConfigurationPath(string configurationFilename) => Path.Combine(ConfigurationDirectory, configurationFilename);

        public static string ContentPath(string relativePath) => Path.Combine(ContentDirectory, relativePath);

        public static string SavePath(string relativePath) => Path.Combine(SaveDirectory, relativePath);

        /// <summary>
        /// Get path of file data folder.
        /// </summary>
        /// <param name="relativePath">Path related to DataFolder</param>
        /// <returns></returns>
        public static string DataPath(string relativePath) => Path.Combine(DataDirectory, relativePath);

        public static string DataRelativePath(string path)
        {
            var dataFullPath = Path.GetFullPath(DataDirectory);
            var fullPath     = Path.GetFullPath(path);

            return fullPath.Substring(dataFullPath.Length);
        }

        #endregion

        #region Directory

        private void CreateDirectory()
        {
            if (!Directory.Exists(SaveDirectory))
            {
                // If it doesn't exist, create
                Directory.CreateDirectory(SaveDirectory);
            }

            if (!Directory.Exists(DataDirectory))
            {
                Directory.CreateDirectory(DataDirectory);
            }
        }

        public void DeleteSaveDirectory()
        {
            if (Directory.Exists(SaveDirectory))
            {
                Directory.Delete(SaveDirectory, true);
            }
        }

        #endregion


        #region IDisposable

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool IsDisposed { get; set; }

        protected virtual void Dispose(bool disposing) { }

        #endregion

    }
}