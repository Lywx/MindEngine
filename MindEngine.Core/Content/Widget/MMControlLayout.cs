namespace MindEngine.Core.Content.Widget
{
    using System;

    public class MMControlLayout : ICloneable
    {
        public MMControlSize DefaultSize { get; set; }

        public MMControlSize MinimumSize { get; set; }

        public MMControlMargins DefaultMargins { get; set; }

        public MMControlMargins ClientMargins { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}