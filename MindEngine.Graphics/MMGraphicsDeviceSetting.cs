namespace MindEngine.Graphics
{
    using System;

    [Serializable]
    public struct MMGraphicsDeviceSetting
    {
        public int ScreenResolutionIndex { get; set; }

        public bool ScreenFullscreenEnabled { get; set; }

        public int ScreenFPS { get; set; }
    }
}
