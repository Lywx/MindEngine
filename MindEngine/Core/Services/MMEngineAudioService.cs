namespace MindEngine.Core.Services
{
    using System;
    using Audio;

    public sealed class MMEngineAudioService : IMMEngineAudioService
    {
        public MMEngineAudioService(IMMEngineAudio audio)
        {
            if (audio == null)
            {
                throw new ArgumentNullException(nameof(audio));
            }

            this.Audio = audio;
        }

        public IMMEngineAudio Audio { get; }

        public IMMAudioManager Manager => this.Audio.Manager;

        public IMMAudioSettings Settings => this.Audio.Settings;

        public IMMAudioController Controller => this.Audio.Controller;

        public IMMAudioDeviceController DeviceController => this.Audio.DeviceController;
    }
}