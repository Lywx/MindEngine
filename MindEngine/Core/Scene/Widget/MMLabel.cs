namespace MindEngine.Core.Scene.Widget
{
    using Content.Font;
    using Content.Font.Extensions;
    using Content.Widget;
    using Microsoft.Xna.Framework;

    public class MMLabel : MMControl
    {
        public MMLabel()
        {
        }

        #region Entity

        public override string EntityClass => "Label";

        #endregion

        #region Widget

        private readonly string layerTextPropertyString = "TextProproty";

        private MMControlLayer layerTextProperty;

        private MMFont layerTextPropertyFont;

        #endregion

        #region

        protected override void OnDesignLayer()
        {
            base.OnDesignLayer();

            this.layerTextProperty = this.Skin.Layers[this.layerTextPropertyString];
            this.layerTextPropertyFont = this.EngineInterop.Asset.Fonts[this.layerTextProperty.Text.FontName];
        }

        #endregion

        protected override void DrawControl(GameTime time)
        {
            base.DrawControl(time);

            var text = StringUtils.BreakStringByWord(this.layerTextPropertyFont, this.Text, 
                this.layerTextProperty.Text.FontSize(this.Skin.Settings.FontScale), 
                this.ClientBounds.Width, 
                this.layerTextProperty.Text.FontMonospaced);

            this.EngineRenderer.DrawString(
                this.layerTextPropertyFont, 
                text,
                this.ClientBounds.Position,
                this.layerTextProperty.Text.FontColor, 
                this.layerTextProperty.Text.FontSize(this.Skin.Settings.FontScale), 
                this.layerTextProperty.Text.HorizontalAlignment, 
                this.layerTextProperty.Text.VerticalAlignment, 
                this.layerTextProperty.Text.FontLeading(this.Skin.Settings.FontScale));
        }
    }
}
