namespace MindEngine.Core.Scenes
{
    using System.Runtime.Serialization;
    using Graphics;
    using Microsoft.Xna.Framework.Graphics;
    using Services;

    /// <summary>
    /// Object for engine service access.
    /// </summary>
    [DataContract]
    public class MMObject
    {
        protected IMMEngine Engine => this.EngineInterop.Engine;

        #region Graphics

        protected IMMEngineGraphicsService EngineGraphics => MMEngine.Service.Graphics;

        protected GraphicsDevice EngineGraphicsDevice => this.EngineGraphics.Device;

        protected IMMRenderer EngineGraphicsRenderer => this.EngineGraphics.Renderer;

        protected IMMGraphicsDeviceController EngineGraphicsDeviceController => this.EngineGraphics.DeviceController;

        #endregion

        protected IMMEngineAudioService EngineAudio => MMEngine.Service.Audio;

        protected IMMEngineInteropService EngineInterop => MMEngine.Service.Interop;

        protected IMMEngineInputService EngineInput => MMEngine.Service.Input;

        protected IMMEngineNumericalService EngineNumerical => MMEngine.Service.Numerical;
    }
}