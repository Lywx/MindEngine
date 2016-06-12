namespace MindEngine.Core.Services.Processes
{
    public enum MMProcessState
    {
        // Neither dead or alive
        Uninitialized,

        Removed,

        // Living Processes
        Running,

        Paused,

        // Dead Processes
        Succeeded,

        Failed,

        Aborted
    }
}
