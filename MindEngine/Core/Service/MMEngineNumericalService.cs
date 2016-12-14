namespace MindEngine.Core.Service
{
    using System;

    /// <summary>
    /// Provide a wrapper around the service in order to hot swap the core 
    /// module in engine.
    /// </summary>
    public class MMEngineNumericalService : IMMEngineNumericalService
    {
        private readonly IMMEngineNumerical numerical;

        public MMEngineNumericalService(IMMEngineNumerical numerical)
        {
            if (numerical == null)
            {
                throw new ArgumentNullException(nameof(numerical));
            }

            this.numerical = numerical;
        }

        public Random Random => this.numerical.Random;
    }
}