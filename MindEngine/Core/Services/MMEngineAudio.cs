namespace MindEngine.Core.Services
{
    using System;

    public class MMEngineAudio : MMGeneralComponent, IMMEngineAudio
    {
        public MMEngineAudio(MMEngine engine, MMAudioSettings settings) : base(engine)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            this.Settings = settings;

            this.Manager  = new MMAudioManager(engine, settings);

            this.DeviceController = MMAudioDeviceControllerFactory.Create(engine);
            this.Game.Components.Add(this.DeviceController);
            this.Controller = new MMAudioController(engine, this.DeviceController);
            this.Game.Components.Add(this.Controller);

        }

        #region Setting Data

        public MMAudioSettings Settings { get; private set; }

        #endregion

        #region Manager Data

        public IMMAudioManager Manager { get; private set; }

        #endregion

        #region Controller Data

        public IMMAudioController Controller { get; private set; }

        public MMAudioDeviceController DeviceController { get; private set; }

        #endregion
    }
}