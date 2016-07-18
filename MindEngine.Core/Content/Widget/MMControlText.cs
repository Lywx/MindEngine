namespace MindEngine.Core.Content.Widget
{
    using System;
    using Microsoft.Xna.Framework;
    using Text;

    internal interface IMMControlText
    {
        int FontBaseLeading { get; set; }

        float FontBaseSize { get; set; }

        Color FontColor { get; set; }

        bool FontMonospaced { get; set; }

        string FontName { get; set; }

        MMHorizontalAlignment HorizontalAlignment { get; set; }

        MMVerticalAlignment VerticalAlignment { get; set; }

        /// <summary>
        /// The font leading depends on the scale context which may be provided 
        /// in the global skin or settings.
        /// </summary>
        int FontLeading(float scale);

        /// <summary>
        /// The font size depends on the scale context which may be provided in 
        /// the global skin or settings.
        /// </summary>
        float FontSize(float scale);
    }

    public class MMControlText : IMMControlText, ICloneable
    {
        public Color FontColor { get; set; }

        public string FontName { get; set; }

        public float FontBaseSize { get; set; }

        public int FontBaseLeading { get; set; } = 0;

        public bool FontMonospaced { get; set; } = false;

        public MMHorizontalAlignment HorizontalAlignment { get; set; } = MMHorizontalAlignment.Right;

        public MMVerticalAlignment VerticalAlignment { get; set; } = MMVerticalAlignment.Bottom;

        public float FontSize(float scale)
        {
            return this.FontBaseSize * scale;
        }

        public int FontLeading(float scale)
        {
            return (int)(this.FontBaseLeading * scale);
        }

        public object Clone()
        {
            var clone = (MMControlText)this.MemberwiseClone();

            clone.FontName = string.Copy(this.FontName);

            return clone;
        }
    }
}
