namespace MindEngine.Core.Scenes
{
    using System;

    public class MMScreenSettings : ICloneable
    {
        public bool AlwaysDraw = false;

        /// <remarks>
        /// Helpful for debugging update issues
        /// </remarks>>
        public bool AlwaysActive  = true;

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}