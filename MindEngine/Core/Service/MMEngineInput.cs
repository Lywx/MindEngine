namespace MindEngine.Core.Service
{
    using Component;
    using Input;
    using Input.Keyboard;
    using Input.Mouse;
    using Microsoft.Xna.Framework;

    public class MMEngineInput : MMCompositeComponent, IMMEngineInput
    {
        #region Constructors

        public MMEngineInput(MMEngine engine)
            : base(engine)
        {
            this.Mouse    = new MMMouseInput(engine);
            this.Keyboard = new MMKeyboardInput();
        }

        #endregion Constructors

        /// <summary>
        ///     Sets or gets input methods allowed for navigation.
        /// </summary>
        private MMInputDevice Device { get; } = MMInputDevice.All;

        public IMMMouseInput Mouse { get; }

        public IMMKeyboardInput Keyboard { get; private set; }

        public override void UpdateInput(GameTime time)
        {
            if ((this.Device & MMInputDevice.Mouse) == MMInputDevice.Mouse)
            {
                this.Mouse.UpdateInput(time);
            }

            if ((this.Device & MMInputDevice.Keyboard) == MMInputDevice.Keyboard)
            {
                this.Keyboard.UpdateInput(time);
            }
        }

        #region Action Bindings

        public void LoadKeyboardBinding<TActions>() where TActions : MMInputActions
        {
            this.Keyboard = new MMKeyboardInput(MMKeyboardBindingUtils.Load<TActions>(@"Control (Keyboard).ini"));
        }

        // public void LoadMouseBinding<TActions>() where TActions : MMInputActions
        // {
        //     // This feature is not supported now
        //     this.Mouse = new MMMouseInput(MMMouseBindingUtils.Load<TActions>(@"Control (Mouse).ini"));
        // }

        #endregion

        #region Action States

        public bool ActionPressed(MMInputAction action)
        {
            // You could add keyboard and mouse action press logic here
            return this.Keyboard.ActionPressed(action);
        }

        public ulong ActionPressedDuration(MMInputAction action)
        {
            // You could add keyboard and mouse action press logic here
            return this.Keyboard.ActionPressedDuration(action);
        }

        public bool ActionUp(MMInputAction action)
        {
            // You could add keyboard and mouse action press logic here
            return this.Keyboard.ActionUp(action);
        }

        public bool ActionDown(MMInputAction action)
        {
            // You could add keyboard and mouse action press logic here
            return this.Keyboard.ActionDown(action);
        }

        #endregion
    }
}
