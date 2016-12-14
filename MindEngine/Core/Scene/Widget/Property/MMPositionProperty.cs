namespace MindEngine.Core.Scene.Widget.Property
{
    using System;
    using Microsoft.Xna.Framework;

    public interface IMMPositionProperty : ICloneable
    {
        int Height { get; set; }

        Vector2 Offset { get; }

        Point Size { get; }

        int Width { get; set; }

        int X { get; set; }

        int Y { get; set; }

        int Z { get; set; }
    }

    public class MMPositionProperty : IMMPositionProperty
    {
        /// <summary>
        /// Relative offset in x direction.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Relative offset in y direction.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Relative offset in z direction. The higher the fronter drawn in the 
        /// screen.
        /// </summary>
        public int Z { get; set; }

        public Vector2 Offset => new Vector2(this.X, this.Y);

        public int Width { get; set; }

        public int Height { get; set; }

        public Point Size => new Point(this.Width, this.Height);

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}