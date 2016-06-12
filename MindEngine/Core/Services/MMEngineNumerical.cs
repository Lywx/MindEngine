namespace MindEngine.Core.Services
{
    using System;

    public class MMEngineNumerical : IMMEngineNumerical
    {
        public MMEngineNumerical()
        {
            this.Random = new Random((int)DateTime.Now.Ticks);
        }

        public Random Random { get; private set; }

        public void Initialize()
        {
        }

        public void Dispose()
        {
        }
    }
}