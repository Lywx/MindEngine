using System.Windows.Forms;

namespace MindEngine.Graphics
{
    public interface IMMGraphicsSettings
    {
        float Aspect { get; set; }

        int Fps { get; set; }

        int Height { get; set; }

        bool IsFullscreen { get; set; }

        bool IsWindowMode { get; set; }

        Screen Screen { get; }

        int ScreenHeight { get; }

        int ScreenWidth { get; }

        int Width { get; set; }
    }
}