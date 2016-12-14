namespace MindEngine.Core.Scene.Widget.Geometry
{
    using System;

    public struct MMSize : IEquatable<MMSize>, ICloneable
    {
        public static MMSize Zero => new MMSize(0, 0);

        public readonly int Width;

        public readonly int Height;

        public MMSize(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        public bool Equals(MMSize other)
        {
             return this.Width.Equals(other.Width) && this.Height.Equals(other.Height);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}