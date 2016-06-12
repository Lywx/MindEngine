namespace MindEngine.Core.Services.Processes
{
    using Components;

    public interface IMMProcessManager : IMMGameComponent
    {
        void AbortProcesses(bool immediate);

        void AttachProcess(IMMProcessManagerItem process);
    }
}