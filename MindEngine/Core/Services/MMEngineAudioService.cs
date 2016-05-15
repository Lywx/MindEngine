namespace MindEngine.Core.Services
{
    using System;

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

        public MMAudioSettings Settings => this.Audio.Settings;

        public IMMAudioController Controller => this.Audio.Controller;

        public MMAudioDeviceController DeviceController => this.Audio.DeviceController;
    }
}