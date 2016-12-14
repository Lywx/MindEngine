namespace MindEngine.Core
{
    using Component;

    /// <summary>
    /// Game is used in engine as the only extra component that update and draw.
    /// </summary>
    public class MMGame : MMCompositeComponent, IMMGame
    {
        #region Constructors

        protected MMGame(MMEngine engine)
            : base(engine)
        {
            EngineInterop.Game.Add(this);
        }

        #endregion

        #region IMMGame

        public virtual void OnExit() {}

        public void Run()
        {
            this.Engine.Run();
        }

        #endregion
    }
}
