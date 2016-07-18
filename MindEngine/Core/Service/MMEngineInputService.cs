namespace MindEngine.Core.Service
{
    using System;
    using Input;
    using Input.Keyboard;
    using Input.Mouse;
    using Microsoft.Xna.Framework;

    public sealed class MMEngineInputService : IMMEngineInputService
    {
        public MMEngineInputService(IMMEngineInput input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            this.Input = input;
        }

        private IMMEngineInput Input { get; }

        public void UpdateInput(GameTime time)
        {
            this.Input.UpdateInput(time);
        }

        public IMMKeyboardInput Keyboard => this.Input.Keyboard;

        public IMMMouseInput Mouse => this.Input.Mouse;

        public bool ActionPressed(MMInputAction action)
        {
            return this.Input.ActionPressed(action);
        }

        public ulong ActionPressedDuration(MMInputAction action)
        {
            return this.Input.ActionPressedDuration(action);
        }

        public bool ActionDown(MMInputAction action)
        {
            return this.Input.ActionDown(action);
        }

        public bool ActionUp(MMInputAction action)
        {
            return this.Input.ActionUp(action);
        }

        public void LoadKeyboardBinding<TActions>() where TActions : MMInputActions
        {
            this.Input.LoadKeyboardBinding<TActions>();
        }
    }
}
