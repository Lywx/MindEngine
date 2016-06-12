namespace MindEngine.Core.Services.Processes
{
    public interface IMMProcess : IMMProcessManagerItem
    {
        string Name { get; set; }

        #region Process Chain Semantics

        IMMProcess Child { get; }

        void AttachChild(IMMProcess process);

        #endregion
    }
}
