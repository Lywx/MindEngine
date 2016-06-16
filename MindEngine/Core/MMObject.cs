namespace MindEngine.Core
{
    using System;
    using Graphics;
    using Microsoft.Xna.Framework.Graphics;
    using Services;

    /// <summary>
    ///     Object for engine service access.
    /// </summary>
    [Serializable]
    public class MMObject
    {
        #region Engine Access

        protected IMMEngine Engine => this.EngineInterop.Engine;

        protected IMMEngineAudioService EngineAudio => MMEngine.Service.Audio;

        protected IMMEngineInteropService EngineInterop => MMEngine.Service.Interop;

        protected IMMEngineInputService EngineInput => MMEngine.Service.Input;

        protected IMMEngineNumericalService EngineNumerical => MMEngine.Service.Numerical;

        protected IMMEngineGraphicsService EngineGraphics => MMEngine.Service.Graphics;

        protected GraphicsDevice EngineGraphicsDevice => this.EngineGraphics.Device;

        protected IMMRenderer EngineGraphicsRenderer => this.EngineGraphics.Renderer;

        protected IMMGraphicsDeviceController EngineGraphicsDeviceController => this.EngineGraphics.DeviceController;

        #endregion
    }
}
