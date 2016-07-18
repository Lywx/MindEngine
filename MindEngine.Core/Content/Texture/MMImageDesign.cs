namespace MindEngine.Core.Content.Texture
{
    /// <summary>
    /// This class is used to provide designer settings for resolution auto-adjustment. 
    /// </summary>
    public struct MMImageDesign
    {
        public MMImageDesign(int designWidth, int designHeight, int screenWidth, int screenHeight)
        {
            this.DesignWidth  = designWidth;
            this.DesignHeight = designHeight;
            this.ScreenWidth  = screenWidth;
            this.ScreenHeight = screenHeight;
        }

        public int DesignWidth { get; private set; }

        public float DesignWidthRatio => (float)this.DesignWidth / (float)this.ScreenWidth;

        public int DesignHeight { get; private set; }

        public float DesignHeightRatio => (float)this.DesignHeight / (float)this.ScreenHeight;

        public int ScreenWidth { get; private set; }

        public int ScreenHeight { get; private set; }
    }
}