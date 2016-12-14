namespace MindEngine.Core.Service
{
    using Component;
    using Graphics;
    using Microsoft.Xna.Framework.Graphics;

    internal class MMEngineNullGraphics : MMCompositeComponent, IMMEngineGraphics
    {
        public IMMGraphicsDeviceManager DeviceManager { get; }

        public IMMGraphicsSettings Settings { get; }

        public MMGraphicsRenderer Renderer { get; }

        public IMMGraphicsDeviceController DeviceController { get; }

        public GraphicsDevice Device { get; }

        public MMCursorDevice Cursor { get; set; }

        public MMEngineNullGraphics(MMEngine engine) : base(engine)
        {
            
        }
    }
}
