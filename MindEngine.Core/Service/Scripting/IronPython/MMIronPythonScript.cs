namespace MindEngine.Core.Service.Scripting.IronPython
{
    public interface IMMIronPythonScript
    {
        void Update(MMIronPythonSession session);

        string Path { get; set; }
    }

    public class MMIronPythonScript : IMMIronPythonScript
    {
        
    }
}