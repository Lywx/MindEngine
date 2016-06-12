namespace MindEngine.Core
{
    using Components;
    using Microsoft.Xna.Framework;

    public interface IMMGame : IMMGameComponent, IUpdateable, IDrawable
    {
        void Run();

        void OnExit();
    }
}