namespace MindEngine.Graphics
{
    using Microsoft.Xna.Framework.Graphics;

    public interface IMMGraphicsManager
    {
        GraphicsDevice GraphicsDevice { get; }

        void ApplySettings(MMGraphicsSettings settings);
    }
}