namespace MindEngine.Core.Scene.Widget.Property
{
    using System;
    using Microsoft.Xna.Framework;

    internal interface IMMWireframeProperty : ICloneable
    {
        Color LineColor { get; set; }

        float LineThick { get; set; }

        Color FillColor { get; set; }
    }

    public class MMWireframeProperty : IMMWireframeProperty
    {
        public Color LineColor { get; set; } = Color.Transparent;

        public float LineThick { get; set; } = 1f;

        public Color FillColor { get; set; } = Color.Transparent;

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}