namespace MindEngine.Core
{
    using System;
    using Service;

    public interface IMMEngineOperations
    {
        void Run();

        void Restart();
    }

    public interface IMMEngine : IDisposable, IMMEngineOperations
    {
        IMMEngineAudio Audio { get; } 

        IMMEngineGraphics Graphics { get; }

        IMMEngineInput Input { get; }

        IMMEngineInterop Interop { get; }

        IMMEngineNumerical Numerical { get; }

        IMMEngineDebug Debug { get; set; }
    }
}
