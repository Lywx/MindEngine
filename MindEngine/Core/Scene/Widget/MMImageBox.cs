namespace MindEngine.Core.Scene.Widget
{
    using System;
    using System.Diagnostics;
    using Content.Font;
    using Content.Texture;
    using Content.Widget;
    using Microsoft.Xna.Framework;
    using Util;

    public interface IMMImageBox
    {
        MMImage Image { get; set; }

        Color ImageDrawColor { get; }

        byte ImageOpacity { get; set; }

        Color ImageTinkColor { get; set; }
    }

    public class MMImageBox : MMControl, IMMImageBox
    {
        public MMImageBox()
        {
        }

        public MMImageBox(MMImage image)
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            this.Image = image;
        }

        #region Entity

        public override string EntityClass => "ImageBox";

        #endregion

        #region Layers

#if DEBUG
        private readonly string layerDebugImagePropertyString = "DebugImageProperty";

        private MMFont layerDebugImagePropertyFont;

        private MMControlLayer layerDebugImageProperty;
#endif

        #endregion

        #region Widget

        private byte imageOpacity = 255;

        private Color imageDrawColor = Color.White;

        private Color imageTinkColor = Color.White;

        public MMImage Image { get; set; }

        public Color ImageDrawColor
        {
            get { return this.imageDrawColor; }
        }

        public Color ImageTinkColor
        {
            get { return this.imageTinkColor; }
            set
            {
                this.imageTinkColor = value;
                this.imageDrawColor = this.imageTinkColor.MakeTransparent(this.imageOpacity); 
            }
        }

        public byte ImageOpacity
        {
            get { return this.imageOpacity; }
            set
            {
                this.imageOpacity = value;
                this.imageDrawColor = this.imageTinkColor.MakeTransparent(this.imageOpacity);
            }
        }

        protected override void OnDesignLayer()
        {
            this.OnDesignLayerDebug();
        }

        [Conditional("DEBUG")]
        protected virtual void OnDesignLayerDebug()
        {
            this.layerDebugImageProperty = this.Manager.Skin.Layers[this.layerDebugImagePropertyString];
            this.layerDebugImagePropertyFont = this.EngineInterop.Asset.Fonts[this.layerDebugImageProperty.Text.FontName];
        }

        #endregion

        protected override void DrawControl(GameTime time)
        {
            base.DrawControl(time);

            if (this.Image != null)
            {
                this.EngineRenderer.Draw(this.Image.Resource, this.Bounds.Rectangle, this.imageDrawColor, this.DrawDepth);
            }
        }

        protected override void DrawControlProperty(GameTime time)
        {
            base.DrawControlProperty(time);
           
            if (this.Engine.Debug.Graphics_WidgetPrimitiveEnabled)
            {
                this.EngineRenderer.DrawMonospacedString(
                    this.layerDebugImagePropertyFont, 
                    $"Opacity = {this.ImageOpacity}\nColor = {this.imageDrawColor}",
                    this.layerDebugImageProperty.Offset + this.Bounds.Position.ToVector2(), 
                    this.layerDebugImageProperty.Text.FontColor, 
                    this.layerDebugImageProperty.Text.FontSize(this.Skin.Settings.FontScale));
            }
        }
    }
}