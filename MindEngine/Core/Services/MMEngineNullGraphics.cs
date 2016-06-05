namespace MindEngine.Core.Services
{
    using Graphics;
    using Microsoft.Xna.Framework.Graphics;

    internal class MMEngineNullGraphics : IMMEngineGraphics
    {
        public IMMGraphicsManager Manager { get; }

        public IMMGraphicsSettings Settings { get; }

        public IMMRenderer Renderer { get; }

        public IMMGraphicsDeviceController DeviceController { get; }

        public GraphicsDevice Device { get; }

        public void Initialize()
        {
        }

        public void Dispose()
        {
        }
    }
}