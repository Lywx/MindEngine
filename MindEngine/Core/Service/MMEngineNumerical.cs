namespace MindEngine.Core.Service
{
    using System;
    using Microsoft.Xna.Framework;

    public class MMEngineNumerical : IMMEngineNumerical
    {
        public MMEngineNumerical()
        {
            this.Random = new Random((int)DateTime.Now.Ticks);
        }

        public Random Random { get; }

        public void Initialize() {}

        public void Dispose() {}

        public void Update(GameTime time) {}
    }
}
