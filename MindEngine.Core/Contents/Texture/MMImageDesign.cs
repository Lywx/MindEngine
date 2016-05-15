namespace MindEngine.Core.Contents.Texture
{
    /// <summary>
    /// This class is used to provide designer settings for resolution auto-adjustment. 
    /// </summary>
    public struct MMImageDesign
    {
        public MMImageDesign(int screenWidth, int screenHeight)
        {
            this.ScreenWidth  = screenWidth;
            this.ScreenHeight = screenHeight;
        }

        public int ScreenWidth { get; private set; }

        public int ScreenHeight { get; private set; }
    }
}