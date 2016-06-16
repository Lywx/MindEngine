namespace MindEngine.Core
{
    using Components;
    using Graphics;
    using Microsoft.Xna.Framework.Graphics;

    public interface IMMGraphicsManager
    {
        void Initialize();

        GraphicsDevice GraphicsDevice { get; }

        void ApplySettings(MMGraphicsSettings settings);
    }
}
