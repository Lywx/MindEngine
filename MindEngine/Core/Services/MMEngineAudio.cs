namespace MindEngine.Core.Services
{
    using System;
    using Audio;
    using Components;

    public class MMEngineAudio : MMCompositeComponent, IMMEngineAudio
    {
        public MMEngineAudio(MMEngine engine, MMAudioSettings settings) : base(engine)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            this.Settings = settings;

            //TODO(Wuxiang)
            //this.Manager = new MMAudioManager(engine, settings);

            //TODO(Wuxiang)
            //this.DeviceController = MMAudioDeviceControllerFactory.Create(engine);
            //this.Game.Components.Add(this.DeviceController);
            //this.Controller = new MMAudioController(engine, this.DeviceController);
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