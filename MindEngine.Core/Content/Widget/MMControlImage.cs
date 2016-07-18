namespace MindEngine.Core.Content.Widget
{
    using System;

    public class MMControlImage : ICloneable
    {
        public string Name { get; set; }

        // TODO: Stretch policy

        public object Clone()
        {
            var clone = (MMControlImage)this.MemberwiseClone();

            clone.Name = string.Copy(this.Name);

            return clone;
        }
    }
}