namespace MindEngine.Core.Components
{
    using System;
    using Microsoft.Xna.Framework;
    using NLog;

    public class MMGameManager : GameComponent, IMMGameManager
    {
#if DEBUG
        #region Logger

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        #endregion
#endif

        #region Constructors and Finalizer

        public MMGameManager(MMEngine engine) 
            : base(engine)
        {
            if (engine == null)
            {
                throw new ArgumentNullException(nameof(engine));
            }

            this.Engine = engine;
        }

        #endregion

        public new IMMGame Game { get; private set; }

        protected MMEngine Engine { get; set; }

        public void Add(IMMGame game)
        {
            if (game == null)
            {
                throw new ArgumentNullException(nameof(game));
            }

            if (this.Game != null)
            {
                throw new InvalidOperationException("Game exists already.");
            }

            this.Game = game;

            this.Engine.Components.Add(game);
#if DEBUG
            logger.Info($"Added {this} to GameEngine.Components.");
#endif
        }

        public void OnExit()
        {
            this.Game?.OnExit();
        }

        #region IDisposable

        private bool IsDisposed { get; set; }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (!this.IsDisposed)
                    {
                        this.Game?.Dispose();
                        this.Game = null;
                    }

                    this.IsDisposed = true;
                }
            }
            catch
            {
                // Ignored
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        #endregion
    }
}