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
        protected IMMEngine GlobalEngine => this.GlobalInterop.Engine;

        #region Graphics

        protected IMMEngineGraphicsService GlobalGraphics => MMEngine.Service.Graphics;

        protected GraphicsDevice GlobalGraphicsDevice => this.GlobalGraphics.Device;

        protected IMMRenderer GlobalGraphicsRenderer => this.GlobalGraphics.Renderer;

        protected IMMGraphicsDeviceController GlobalGraphicsDeviceController => this.GlobalGraphics.DeviceController;

        #endregion

        protected IMMEngineAudioService GlobalAudio => MMEngine.Service.Audio;

        protected IMMEngineInteropService GlobalInterop => MMEngine.Service.Interop;

        protected IMMEngineInputService GlobalInput => MMEngine.Service.Input;

        protected IMMEngineNumericalService GlobalNumerical => MMEngine.Service.Numerical;
    }
}