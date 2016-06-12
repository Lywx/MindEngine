namespace MindEngine.Input
{
    using Core;
    using Core.Components;
    using Keyboard;
    using Microsoft.Xna.Framework;
    using Mouse;

    public class MMInputState : MMCompositeComponent, IMMInputState
    {
        #region Constructors

        public MMInputState(MMEngine engine)
            : base(engine)
        {
            this.Mouse    = new MMMouseInput();
            this.Keyboard = new MMKeyboardInput();
        }

        #endregion Constructors

        public IMMMouseInput Mouse { get; private set; }

        public IMMKeyboardInput Keyboard { get; private set; }

        public void LoadKeyboardBinding<TActions>() where TActions : MMInputActions
        {
            this.Keyboard = new MMKeyboardInput(MMKeyboardBindingUtils.Load<TActions>(@"Control (Keyboard).ini"));
        }

        public void LoadMouseBinding<TActions>() where TActions : MMInputActions
        {
            // This feature is not supported
            // this.Mouse = new MMMouseInput(MMMouseBindingUtils.Load<TActions>(@"Control\Mouse.ini"));
        }

        #region Update

        public override void UpdateInput(GameTime time)
        {
            this.Mouse   .UpdateInput(time);
            this.Keyboard.UpdateInput(time);
        }

        #endregion  
    }
}
