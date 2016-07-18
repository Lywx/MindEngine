namespace MindEngine.Core.Content.Widget
{
    using System;

    public struct MMControlMargins : IEquatable<MMControlMargins>, ICloneable
    {
        /// <summary>
        ///     Bottom margin.
        /// </summary>
        public readonly int Bottom;

        /// <summary>
        ///     Left margin.
        /// </summary>
        public readonly int Left;

        /// <summary>
        ///     Right margin.
        /// </summary>
        public readonly int Right;

        /// <summary>
        ///     Top margin.
        /// </summary>
        public readonly int Top;

        public MMControlMargins(int left, int top, int right, int bottom)
        {
            this.Left = left;
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
        }

        /// <summary>
        ///     Vertical margin sum.
        /// </summary>
        public int Vertical => this.Top + this.Bottom;

        /// <summary>
        ///     Horizontal margin sum.
        /// </summary>
        public int Horizontal => this.Left + this.Right;

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public bool Equals(MMControlMargins other)
        {
            return this.Left == other.Left
                   && this.Top == other.Top
                   && this.Right == other.Right
                   && this.Bottom == other.Bottom;
        }
    }
}
