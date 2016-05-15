namespace MindEngine.Graphics
{
    using System;
    using System.Linq;
    using System.Windows.Forms;
    using IO.Configuration;
    using Math;
    using NLog;

    /// <summary>
    ///     Graphics settings data.
    /// </summary>
    public class MMGraphicsSettings : IMMGraphicsSettings
    {
#if DEBUG
        #region Logger

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        #endregion
#endif

        #region Constructors

        public MMGraphicsSettings()
        {
            this.LoadConfiguration();
        }

        #endregion

        #region Configuration

        public void LoadConfiguration()
        {
            var configuration = MMPlainConfigurationLoader.LoadUnique("Graphics.ini");

            this.Aspect = (float)
                MMPlainConfigurationReader.ReadValueInts(configuration, "Resolution Aspect", 0, 16) 
                / MMPlainConfigurationReader.ReadValueInts(configuration, "Resolution Aspect", 1, 9);

            this.SetResolution(
                MMPlainConfigurationReader.ReadValueInts(configuration, "Resolution", 0, 800), 
                MMPlainConfigurationReader.ReadValueInts(configuration, "Resolution", 1, 600));

            this.IsFullscreen = MMPlainConfigurationReader.ReadValueBool(configuration, "Full Screen", false);
            this.IsWindowMode = MMPlainConfigurationReader.ReadValueBool(configuration, "Window Mode", true);

            this.Fps = MMPlainConfigurationReader.ReadValueInt(configuration, "FPS", 30);
        }

        #endregion

        #region Dependency

        public Screen Screen => Screen.AllScreens.First(e => e.Primary);

        public int ScreenWidth => this.Screen.Bounds.Width;

        public int ScreenHeight => this.Screen.Bounds.Height;

        #endregion

        #region Resolution Data 

        private int width;

        private int height;

        public int Height
        {
            get
            {
                return this.IsFullscreen
                           ? this.Screen.Bounds.Height - 1
                           : this.height;
            }

            set { this.height = value; }
        }

        public int Width
        {
            get { return this.IsFullscreen ? this.Screen.Bounds.Width : this.width; }

            set { this.width = value; }
        }

        private void SetResolution(int valueWidth, int valueHeight)
        {
            if (this.IsValidResolution(valueWidth, valueHeight))
            {
                this.Width = valueWidth;
                this.Height = valueHeight;
            }
        }

        public float Aspect { get; set; }

        #endregion

        #region Performance Data

        private int fps;

        public int Fps
        {
            get { return this.fps; }
            set
            {
                if (this.IsValidFps(value))
                {
                    this.fps = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }

        public bool IsFullscreen { get; set; }

        private bool isWindowMode;

        public bool IsWindowMode
        {
            get { return this.isWindowMode; }
            set { this.isWindowMode = !this.IsFullscreen && value; }
        }

        #endregion

        #region Validation

        private bool IsValidAspect(int valueWidth, int valueHeight)
        {
            var aspect = ((double)valueWidth / valueHeight);

            if (this.IsNearlyEqual(aspect, 16.0 / 9.0))
            {
                return false;
            }

            return true;
        }

        private bool IsValidResolution(int valueWidth, int valueHeight)
        {
            if (valueWidth < 800
                || valueHeight < 600)
            {
                return false;
            }

            return true;
        }

        private bool IsValidFps(int valueFps)
        {
            if (valueFps < 15
                || valueFps > 60)
            {
                return false;
            }

            return true;
        }

        private bool IsNearlyEqual(double value, double valueValid)
        {
            // 0.1 as epsilon is enough
            return !value.NearlyEqual(valueValid, 0.1);
        }

        #endregion
    }
}
