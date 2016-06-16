namespace MindEngine.Core.Services.Process
{
    /// <remarks>
    /// The smaller the higher priority.
    /// </remarks>
    public enum MMProcessCategory
    {
        System_Audio = 4000,

        System_Graphics = 2000,

        System_Profiler = 5000,

        System_IO = 3000,

        System_Input = 0,
    
        User = 1000,
    }
}