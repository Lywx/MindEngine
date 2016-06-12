namespace MindEngine.Core.Services
{
    using Components;
    using Graphics;
    using Microsoft.Xna.Framework.Graphics;

    internal class MMEngineNullGraphics : MMCompositeComponent, IMMEngineGraphics
    {
        public IMMGraphicsManager Manager { get; }

        public IMMGraphicsSettings Settings { get; }

        public IMMRenderer Renderer { get; }

        public IMMGraphicsDeviceController DeviceController { get; }

        public GraphicsDevice Device { get; }

        public MMCursorDevice Cursor { get; set; }

        public MMEngineNullGraphics(MMEngine engine) : base(engine)
        {
            
        }
    }
}
