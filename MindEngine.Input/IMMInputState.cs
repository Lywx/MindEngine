namespace MindEngine.Input
{
    using Core.Components;
    using Keyboard;
    using Mouse;

    public interface IMMInputState : IMMCompositeComponent 
    {
        IMMKeyboardInput Keyboard { get; }

        IMMMouseInput Mouse { get; }

        void LoadKeyboardBinding<TActions>() where TActions : MMInputActions;

        void LoadMouseBinding<TActions>() where TActions : MMInputActions; 
    }
}