namespace MindEngine.Graphics
{
    public class MMGraphicsDeviceSettingProvider
    {
        public MMGraphicsDeviceSettingProvider()
        {
        }

        public MMGraphicsDeviceSetting DefaultSetting { get; private set; }

        public MMGraphicsDeviceSetting CurrentSetting { get; private set; }

        private int FindScreenResolutionIndex(int width, int height)
        {
            return 0;
        }

        public MMGraphicsDeviceSetting Initialize(MMGraphicsDeviceContext deviceContext)
        {
            this.DefaultSetting = new MMGraphicsDeviceSetting
            {
                ScreenFullscreenEnabled = true,
                ScreenResolutionIndex = this.FindScreenResolutionIndex(
                    deviceContext.ScreenPrimary.Bounds.Width,
                    deviceContext.ScreenPrimary.Bounds.Height),
                ScreenFPS = 60,

            };

            return this.DefaultSetting;
        }
    }
}