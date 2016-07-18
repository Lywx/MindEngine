namespace MindEngine.Core
{
    using Component;
    using Microsoft.Xna.Framework;

    public interface IMMGame : IMMGameComponent, IUpdateable, IDrawable
    {
        void Run();

        void OnExit();
    }
}