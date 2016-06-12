﻿namespace MindEngine.Core.Services
{
    using Audio;

    internal class MMEngineNullAudio : IMMEngineAudio
    {
        public IMMAudioManager Manager { get; }

        public IMMAudioSettings Settings { get; }

        public IMMAudioController Controller { get; }

        public IMMAudioDeviceController DeviceController { get; }

        public MMEngineNullAudio()
        {
            
        }
    }
}