namespace MindEngine.Core.Service
{
    using System;
    using Audio;

    /// <summary>
    /// Provide a wrapper around the service in order to hot swap the core 
    /// module in engine.
    /// </summary>
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

        private IMMEngineAudio Audio { get; }

        public IMMAudioManager Manager => this.Audio.Manager;

        public IMMAudioSettings Settings => this.Audio.Settings;

        public IMMAudioController Controller => this.Audio.Controller;

        public IMMAudioDeviceController DeviceController => this.Audio.DeviceController;
    }
}