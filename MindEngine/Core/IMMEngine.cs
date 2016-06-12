namespace MindEngine.Core
{
    using System;
    using Services;

    public interface IMMEngine : IDisposable, IMMEngineOperations
    {
        IMMEngineInput Input { get; }

        IMMEngineInterop Interop { get; }

        IMMEngineGraphics Graphics { get; }
    }
}
