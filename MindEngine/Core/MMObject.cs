namespace MindEngine.Core
{
    using System;
    using Graphics;
    using Service;

    /// <summary>
    ///     Object for engine service access.
    /// </summary>
    [Serializable]
    public class MMObject
    {
        #region Engine Access

        protected static IMMEngine Engine => EngineInterop.Engine;

        protected static IMMEngineAudioService EngineAudio => MMEngine.Service.Audio;

        protected static IMMEngineDebugService EngineDebug => MMEngine.Service.Debug;

        protected static IMMEngineInteropService EngineInterop => MMEngine.Service.Interop;

        protected static IMMEngineInputService EngineInput => MMEngine.Service.Input;

        protected static IMMEngineNumericalService EngineNumerical => MMEngine.Service.Numerical;

        protected static IMMEngineGraphicsService EngineGraphics => MMEngine.Service.Graphics;

        protected static MMGraphicsRenderer EngineRenderer => EngineGraphics.Renderer;

        protected static IMMGraphicsDeviceController EngineGraphicsDeviceController => EngineGraphics.DeviceController;

        #endregion
    }
}
