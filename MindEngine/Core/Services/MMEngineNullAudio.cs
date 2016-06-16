namespace MindEngine.Core.Services
{
    using Audio;
    using Microsoft.Xna.Framework;

    internal class MMEngineNullAudio : IMMEngineAudio
    {
        public IMMAudioManager Manager { get; }

        public IMMAudioSettings Settings { get; }

        public IMMAudioController Controller { get; }

        public IMMAudioDeviceController DeviceController { get; }

        public void Update(GameTime time) {}
    }
}
