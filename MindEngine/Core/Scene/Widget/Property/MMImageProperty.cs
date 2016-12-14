namespace MindEngine.Core.Scene.Widget.Property
{
    using System;
    using Microsoft.Xna.Framework;
    using Util;

    internal interface IMMImageProperty : ICloneable 
    {
        string ImageName { get; set; }

        Color ImageBaseColor { get; set; }

        Color ImageColor(byte opacity);

        byte ImageOpacity { get; set; }
    }

    public class MMImageProperty : IMMImageProperty
    {
        public string ImageName { get; set; }

        public byte ImageOpacity { get; set; } = byte.MaxValue;

        public Color ImageBaseColor { get; set; } = Color.White;

        public Color ImageColor(byte opacity)
        {
            return this.ImageBaseColor.MakeTransparent(opacity);
        }

        public object Clone()
        {
            var clone = (MMImageProperty)this.MemberwiseClone();

            clone.ImageName = string.Copy(this.ImageName);

            return clone;
        }
    }
}