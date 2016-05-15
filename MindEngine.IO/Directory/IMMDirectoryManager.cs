namespace MindEngine.IO.Directory
{
    using System;

    public interface IMMDirectoryManager : IDisposable
    {
        void DeleteSaveDirectory();
    }
}