namespace MindEngine.Core.Service.State
{
    using System;

    public class MMScreenSettings : ICloneable
    {
        public bool DrawAlwaysEnabled = false;

        /// <remarks>
        /// Helpful for debugging update issues
        /// </remarks>>
        public bool UpdateAlwaysEnable  = true;

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}