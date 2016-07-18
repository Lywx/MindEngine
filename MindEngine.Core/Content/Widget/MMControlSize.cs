namespace MindEngine.Core.Content.Widget
{
    public struct MMControlSize
    {
        public static MMControlSize Zero => new MMControlSize(0, 0);

        public readonly int Width;

        public readonly int Height;

        public MMControlSize(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }
    }
}