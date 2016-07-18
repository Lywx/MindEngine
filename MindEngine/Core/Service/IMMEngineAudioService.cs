namespace MindEngine.Core.Service
{
    using Audio;

    public interface IMMEngineAudioService
    {
        #region Manager and Settings

        IMMAudioManager Manager { get; }

        IMMAudioSettings Settings { get; }

        #endregion

        #region Controller

        IMMAudioController Controller { get; }

        IMMAudioDeviceController DeviceController { get; }

        #endregion
    }
}