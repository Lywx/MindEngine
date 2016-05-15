namespace MindEngine.Core.Components
{
    using System;
    using Graphics;
    using Microsoft.Xna.Framework;
    using Services;

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

        #region Service Data

        protected IMMEngineInputService GlobalInput => MMEngine.Service.Input;

        protected IMMEngineInteropService GlobalInterop => MMEngine.Service.Interop;

        protected IMMEngineGraphicsService GlobalGraphics => MMEngine.Service.Graphics;

        protected IMMRenderer GlobalGraphicsRenderer => this.GlobalGraphics.Renderer;

        protected IMMEngineNumericalService GlobalNumerical => MMEngine.Service.Numerical;

        #endregion

        #region Draw

        public virtual void BeginDraw(GameTime time)
        {

        }

        public virtual void EndDraw(GameTime time)
        {

        }

        #endregion

        #region Update

        public virtual void UpdateInput(GameTime time)
        {

        }

        #endregion
    }
}