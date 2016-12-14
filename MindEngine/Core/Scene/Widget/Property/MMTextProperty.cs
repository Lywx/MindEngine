namespace MindEngine.Core.Scene.Widget.Property
{
    using System;
    using Content.Text;
    using Microsoft.Xna.Framework;
    using Util;

    internal interface IMMTextProperty
    {
        int FontBaseLeading { get; set; }

        float FontBaseSize { get; set; }

        Color FontBaseColor { get; set; }

        string FontName { get; set; }

        bool FontBold { get; set; }

        bool FontItalic { get; set; }

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

        Color FontColor(byte opacity);

        bool TextMonospaced { get; set; }

        MMHorizontalAlignment TextHorizontalAlignment { get; set; }

        MMVerticalAlignment TextVerticalAlignment { get; set; }
    }

    public class MMTextProperty : IMMTextProperty, ICloneable
    {
        public string FontName { get; set; }

        public bool FontBold { get; set; }

        public bool FontItalic { get; set; }

        public Color FontBaseColor { get; set; }

        public float FontBaseSize { get; set; }

        public int FontBaseLeading { get; set; } = 0;

        public bool TextMonospaced { get; set; } = false;

        public float FontSize(float scale)
        {
            return this.FontBaseSize * scale;
        }

        public Color FontColor(byte opacity)
        {
            return this.FontBaseColor.MakeTransparent(opacity);
        }

        public int FontLeading(float scale)
        {
            return (int)(this.FontBaseLeading * scale);
        }

        public MMHorizontalAlignment TextHorizontalAlignment { get; set; } = MMHorizontalAlignment.Right;

        public MMVerticalAlignment TextVerticalAlignment { get; set; } = MMVerticalAlignment.Bottom;

        public object Clone()
        {
            var clone = (MMTextProperty)this.MemberwiseClone();

            clone.FontName = string.Copy(this.FontName);

            return clone;
        }
    }
}
