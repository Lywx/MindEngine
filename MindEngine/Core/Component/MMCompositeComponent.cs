namespace MindEngine.Core.Component
{
    using System;
    using Microsoft.Xna.Framework;
    using Service;

    public abstract class MMCompositeComponent : DrawableGameComponent, IMMCompositeComponent
    {
        #region Constructors

        protected MMCompositeComponent(MMEngine engine)
            : base(engine)
        {
            if (engine == null)
            {
                throw new ArgumentNullException(nameof(engine));
            }

            this.Engine = engine;
        }

        #endregion

        #region Engine Data

        public MMEngine Engine { get; protected set; }

        #endregion

        #region Update

        public virtual void UpdateInput(GameTime time) {}

        #endregion

        /// The engine component accessors here are available immediately after the 
        /// engine is properly composited. You don't need to initialize engine to 
        /// use engine components.
        /// 
        /// This part is different from the game object engine accessors, which 
        /// only become available after the engine started running.

        #region Engine Component Accessors

        protected IMMEngineInput EngineInput => this.Engine.Input;

        protected IMMEngineInterop EngineInterop => this.Engine.Interop;

        protected IMMEngineGraphics EngineGraphics => this.Engine.Graphics;

        protected IMMEngineNumerical EngineNumerical => this.Engine.Numerical;

        protected IMMEngineAudio EngineAudio => this.Engine.Audio;

        #endregion

        #region Draw

        public virtual void BeginDraw(GameTime time) {}

        public virtual void EndDraw(GameTime time) {}

        #endregion
    }
}
