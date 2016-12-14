namespace MindEngine.Core.Scene.Widget.Component
{
    using Content.Font;
    using Content.Font.Extensions;
    using Control;
    using Microsoft.Xna.Framework;

    public class MMTextComponent : MMControlComponent
    {
        public new class MMElementRegistration : MMControlComponent.MMElementRegistration
        {
            public string Text { get; set; } = "Text";
        }

        public MMTextComponent(MMControl controlParent) 
            : base(controlParent)
        {
            
        }

        public MMTextComponent(MMControl controlParent, MMElementRegistration elementRegistration) 
            : base(controlParent, elementRegistration)
        {
            this.ElementRegistration = elementRegistration;
        }

        #region Element

        private MMElementRegistration elementRegistration;

        public new MMElementRegistration ElementRegistration
        {
            get
            {
                if (this.elementRegistration == null)
                {
                    this.elementRegistration = new MMElementRegistration();
                }

                return this.elementRegistration;
            }
            set
            {
                this.elementRegistration = value;
                base.ElementRegistration = value;
            }
        }

        protected MMTextStyle Element_TextStyle { get; private set; }

        protected MMFont Element_TextFont { get; private set; }

        #endregion

        #region Widget

        protected override void OnInitComponent()
        {
            // You could load layer manually 
            if (this.Element_TextStyle == null)
            {
                this.Element_TextStyle = (MMTextStyle)this.ControlParent.ControlSkin.ElementStyles[this.ElementRegistration.Text];
            }

            this.Element_TextFont = this.EngineInterop.Asset.Fonts[this.Element_TextStyle.TextProperty.FontName];
        }

        #endregion

        #region Draw

        protected override void DrawComponent(GameTime time)
        {
            base.DrawComponent(time);

            var componentBounds = this.ComponentBounds(this.ControlBounds, this.Element_TextStyle);

            var multilineText = StringUtils.BreakStringByWord(
                this.Element_TextFont,
                this.Element_TextStyle.Text,
                this.Element_TextStyle.TextProperty.FontSize(this.ControlParent.ControlSettings.FontScale),
                componentBounds.Width,
                this.Element_TextStyle.TextProperty.TextMonospaced);

            this.EngineRenderer.DrawString(
                this.Element_TextFont,
                multilineText,
                componentBounds.Location,
                this.Element_TextStyle.TextProperty.FontBaseColor,
                this.Element_TextStyle.TextProperty.FontSize(this.ControlParent.ControlSettings.FontScale),
                this.Element_TextStyle.TextProperty.TextHorizontalAlignment,
                this.Element_TextStyle.TextProperty.TextVerticalAlignment,
                this.Element_TextStyle.TextProperty.FontLeading(this.ControlParent.ControlSettings.FontScale));
        }

        #endregion
    }
}
