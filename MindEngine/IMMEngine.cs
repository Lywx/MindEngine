namespace MindEngine
{
    using System;
    using Core.Services;

    public interface IMMEngine : IDisposable, IMMEngineOperations
    {
        IMMEngineInput Input { get; }

        IMMEngineInterop Interop { get; }

        IMMEngineGraphics Graphics { get; }
    }
}