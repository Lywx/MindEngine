namespace MindEngine.Core.Service
{
    using System;

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