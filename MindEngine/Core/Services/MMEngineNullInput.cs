namespace MindEngine.Core.Services
{
    using Components;
    using Input;

    internal class MMEngineNullInput : MMCompositeComponent, IMMEngineInput
    {
        public IMMInputEvent Event { get; private set; }

        public IMMInputState State { get; private set; }

        public MMEngineNullInput(MMEngine engine) 
            : base(engine)
        {
        }
    }
}