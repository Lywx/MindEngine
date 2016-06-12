namespace MindEngine.Core
{
    using Components;
    using Graphics;
    using Microsoft.Xna.Framework.Graphics;

    public interface IMMGraphicsManager : IMMGameComponent
    {
        GraphicsDevice GraphicsDevice { get; }

        void ApplySettings(MMGraphicsSettings settings);
    }
}
