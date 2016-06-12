namespace MindEngine.Core.Services
{
    using System;
    using System.IO;
    using Audio;
    using Components;
    using Microsoft.Xna.Framework.Audio;

    public class MMEngineAudio : MMCompositeComponent, IMMEngineAudio
    {
        public static MMAudioDeviceController CreateDeviceController(MMEngine engine)
        {
            var content = engine.Content.RootDirectory;

            // Current audio requirement is quite easy to use this simple 
            // implementation without relying on asset manager. 
            var audioSettings = Path.Combine(content, @"Audio\Win\Audio.xgs");
            var waveBankSettings = Path.Combine(content, @"Audio\Win\Wave Bank.xwb");
            var soundBankSettings = Path.Combine(content, @"Audio\Win\Sound Bank.xsb");

            var audioEngine = new AudioEngine(audioSettings);
            var waveBank = new WaveBank(audioEngine, waveBankSettings);
            var soundBank = new SoundBank(audioEngine, soundBankSettings);

            return new MMAudioDeviceController(engine, audioEngine, waveBank, soundBank);
        }

        public MMEngineAudio(MMEngine engine, MMAudioSettings settings) : base(engine)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            this.Settings = settings;

            this.Manager = new MMAudioManager(engine, settings);

            this.DeviceController = CreateDeviceController(engine);
            this.Game.Components.Add(this.DeviceController);
            this.Controller = new MMAudioController(engine, this.DeviceController);
            this.Game.Components.Add(this.Controller);

        }

        #region Setting Data

        public IMMAudioSettings Settings { get; private set; }

        #endregion

        #region Manager Data

        public IMMAudioManager Manager { get; private set; }

        #endregion

        #region Controller Data

        public IMMAudioController Controller { get; private set; }

        public IMMAudioDeviceController DeviceController { get; private set; }

        #endregion
    }
}