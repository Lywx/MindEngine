namespace MindEngine.Core.Scene.Widget.Property
{
    using System;
    using Geometry;

    public class MMLayoutProperty : ICloneable
    {
        public MMSize DefaultSize { get; set; }

        public MMSize MinimumSize { get; set; }

        public MMMargins DefaultMargins { get; set; }

        public MMMargins ClientMargins { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}