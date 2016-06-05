#if WINDOWS
namespace MindEngine.IO.Platform
{
    public class MMPlatformPathWin32 : IMMPlatformPath
    {
        public string ConfigurationDirectory => @".\Configuration\";

        public string ContentDirectory       => @".\Content\";

        public string DataDirectory          => @".\Data\";

        public string SaveDirectory          => @".\Save\";
    }
}
#endif 