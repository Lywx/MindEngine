namespace MindEngine.Audio
{
    using IO.Configuration;
    using NLog;

    public class MMAudioSettings : IMMAudioSettings
    {
#if DEBUG

        #region Logger

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        #endregion

#endif

        private int volumeEffect;

        private int volumeMaster;

        private int volumeMusic;

        public MMAudioSettings()
        {
            this.LoadConfiguration();
        }

        /// <summary>
        ///     Sound effect volume.
        /// </summary>
        public int VolumeEffect
        {
            get { return this.volumeEffect; }
            set
            {
                if (this.IsValidVolume(value))
                {
                    this.volumeEffect = value;
                }
            }
        }

        /// <summary>
        ///     Master volume govern other volumes.
        /// </summary>
        public int VolumeMaster
        {
            get { return this.volumeMaster; }
            set
            {
                if (this.IsValidVolume(value))
                {
                    this.volumeMaster = value;
                }
            }
        }

        /// <summary>
        ///     Music volume.
        /// </summary>
        public int VolumeMusic
        {
            get { return this.volumeMusic; }
            set
            {
                if (this.IsValidVolume(value))
                {
                    this.volumeMusic = value;
                }
            }
        }

        #region Configuration

        private bool IsValidVolume(int volume)
        {
            if (100 < volume
                || volume < 0)
            {
                return false;
            }

            return true;
        }

        public void LoadConfiguration()
        {
            var configuration = MMPlainConfigurationLoader.LoadUnique("Audio.ini");

            this.VolumeEffect = MMPlainConfigurationReader.ReadValueInt(configuration, "Effect Volume", 100);
            this.VolumeMaster = MMPlainConfigurationReader.ReadValueInt(configuration, "Master Volume", 100);
            this.VolumeMusic  = MMPlainConfigurationReader.ReadValueInt(configuration, "Music Volume", 100);
        }

        #endregion
    }
}
