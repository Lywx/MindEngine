namespace MindEngine.Core.Services
{
    using System;
    using Input;

    public sealed class MMEngineInputService : IMMEngineInputService
    {
        private readonly IMMEngineInput input;

        public MMEngineInputService(IMMEngineInput input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            this.input = input;
        }

        public IMMInputEvent Event => this.input.Event;

        public IMMInputState State => this.input.State;
    }
}