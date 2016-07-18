namespace MindEngine.Core.Service
{
    using Component;
    using Input;
    using Input.Keyboard;
    using Input.Mouse;

    internal class MMEngineNullInput : MMCompositeComponent, IMMEngineInput
    {
        public MMEngineNullInput(MMEngine engine) 
            : base(engine)
        {
        }

        public IMMKeyboardInput Keyboard { get; }

        public IMMMouseInput Mouse { get; }

        public bool ActionPressed(MMInputAction action)
        {
            return false;
        }

        public ulong ActionPressedDuration(MMInputAction action)
        {
            return 0;
        }

        public bool ActionDown(MMInputAction action)
        {
            return false;
        }

        public bool ActionUp(MMInputAction action)
        {
            return false;
        }

        public void LoadKeyboardBinding<TActions>() where TActions : MMInputActions
        {
        }
    }
}