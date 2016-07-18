using System.Windows.Forms;

namespace MindEngine.Graphics
{
    using Microsoft.Xna.Framework;

    public interface IMMGraphicsSettings
    {
        float Aspect { get; set; }

        int Fps { get; set; }

        int Height { get; set; }

        Point Center { get; }

        bool IsFullscreen { get; set; }

        bool IsBorderless { get; set; }

        Screen Screen { get; }

        int ScreenHeight { get; }

        int ScreenWidth { get; }

        int Width { get; set; }
    }
}