namespace MindEngine.Core.Content.Widget
{
    using System;
    using Microsoft.Xna.Framework;

    public interface IMMControlLayer
    {
        int Height { get; set; }

        Vector2 Offset { get; }

        Vector2 Size { get; }

        MMControlText Text { get; set; }

        MMControlImage Image { get; set; }

        int Width { get; set; }

        int X { get; set; }

        int Y { get; set; }
    }

    /// <summary>
    /// This class describes a rectangular region, in which is a non-interactive
    /// graphical unit. This basically describes a textured rectangle region.
    /// </summary>
    public class MMControlLayer : MMControlElement, ICloneable, IMMControlLayer
    {
        public MMControlLayer(string name) : base(name)
        {
        }

        private MMControlLayer()
        {
        }

        public override void Learn(MMControlElement element)
        {
            base.Learn(element);

            var layer = element as MMControlLayer;
            if (layer != null)
            {
                this.X      = layer.X;
                this.Y      = layer.Y;
                this.Width  = layer.Width;
                this.Height = layer.Height;

                this.Image = (MMControlImage)layer.Image.Clone();
                this.Text  = (MMControlText)layer.Text.Clone();
            }
        }

        /// <summary>
        /// Relative offset in x direction.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Relative offset in y direction.
        /// </summary>
        public int Y { get; set; }

        public Vector2 Offset => new Vector2(this.X, this.Y);

        public int Width { get; set; }

        public int Height { get; set; }

        public Vector2 Size => new Vector2(this.Width, this.Height);

        public MMControlText Text { get; set; }

        public MMControlImage Image { get; set; }

        public override object Clone()
        {
            var clone = new MMControlLayer();
            clone.Copy(this);
            return clone;
        }
    }
}